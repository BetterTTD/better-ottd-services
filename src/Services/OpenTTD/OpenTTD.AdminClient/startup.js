import express from 'express';
import cors from 'cors';
import { HubConnectionBuilder } from '@microsoft/signalr';
import pinoToSeq from 'pino-seq';
import { pinoHttp } from 'pino-http';
import pinoms from 'pino-multi-stream';
import { v4 as uuidv4 } from 'uuid';

function configureHubEvents(hub, storage) {
    hub.on("AskServers", () => {
        hub.send('TellServers', storage.state.map(server => {
            return {
                id: server.id,
                info: {
                    ip: server.instance.info.ip,
                    port: server.instance.info.port
                }
            };
        })).catch((err) => console.log(err));
    });
}

function configureServices(storage) {
    let hubUrl = process.env.DOCKER
        ? 'http://openttd.signalrhub/server'
        : 'http://localhost:6003/server';

    const hubConnection = new HubConnectionBuilder()
        .withUrl(hubUrl)
        .build();

    configureHubEvents(hubConnection, storage);

    const seqUrl = process.env.DOCKER ? 'http://tg.seq:5341' : 'http://localhost:5341';

    const logger = pinoms({
        name: 'OpenTTD.AdminClient',
        streams: [
            { level: 'debug', stream: process.stdout },
            { level: 'error', stream: process.stderr },
            { level: 'debug', stream: pinoToSeq.createStream({ serverUrl: seqUrl }) }
        ]
    });

    return {
        Storage: storage,
        Hub: hubConnection,
        Logger: logger
    };
}

function configureAppRoutes(app, services) {
    const { Hub, Storage, Logger } = services;

    app.get('/ping', (_, res) => {
        res.send('pong');
    });

    app.get('/servers', (_, res) => {
        res.status(200).send(Storage.state.map(server => {
            return {
                id: server.id,
                ip: server.instance.info.ip,
                port: server.instance.info.port
            };
        }));
    });

    app.post('/servers', (req, res) => {
        const { ip, port } = req.body;
        const id = uuidv4();

        if (Storage.state.some(server => server.instance.ip === ip && server.instance.port === port)) {
            res.status(400).send('Server is already added.');
            return;
        }

        Storage.dispatch('SERVER_CREATE', { id, services, ...req.body });
        let hubEvent = { ip: ip, port: port, name: null, version: null, map: null };
        Hub.send('TellServerInfoUpdated', id, hubEvent).catch((err) => console.log(err));
        res.status(200).send(id);
    });

    app.put('/servers/:serverId/connect', (req, res) => {
        let { serverId } = req.params;
        if (!Storage.state.some(s => s.id === serverId)) {
            res.status(404).send('Server not found.');
            return;
        }

        Storage.dispatch('SERVER_CONNECT', { serverId });
        res.status(200).send();
    });

    app.delete('/servers/:serverId/disconnect', (req, res) => {
        let { serverId } = req.params;
        if (!Storage.state.some(s => s.id === serverId)) {
            res.status(404).send('Server not found.');
            return;
        }
        Storage.dispatch('SERVER_DISCONNECT', { serverId });
        res.status(200).send();
    });
}

function configureApplication(services) {
    const { Logger } = services;
    const app = express();

    app.use(cors());
    app.use(express.json());
    app.use(pinoHttp({
        logger: Logger,
        wrapSerializers: true,
        useLevel: 'info',
        customSuccessMessage: function (res) {
            return `[${res.req.method}] ${res.req.url}`;
        }
    }));

    configureAppRoutes(app, services);

    return app;
}

export default {
    ConfigureServices: configureServices,
    ConfigureApplication: configureApplication
};