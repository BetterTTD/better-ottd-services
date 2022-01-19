import express from 'express';
import cors from 'cors';
import { HubConnectionBuilder } from '@microsoft/signalr';
import pinoToSeq from 'pino-seq';
import { pinoHttp } from 'pino-http';
import pinoms from 'pino-multi-stream';
import configureAppRoutes from './appRoutes.js';
import Storage from './storage.js';

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

function configureServices() {
    const seqUrl = process.env.DOCKER ? 'http://tg.seq:5341' : 'http://localhost:5341';
    const logger = pinoms({
        name: 'OpenTTD.AdminClient',
        streams: [
            { level: 'debug', stream: process.stdout },
            { level: 'error', stream: process.stderr },
            { level: 'debug', stream: pinoToSeq.createStream({ serverUrl: seqUrl }) }
        ]
    });
    const hubUrl = process.env.DOCKER
    ? 'http://openttd.signalrhub/server'
    : 'http://localhost:6003/server';

    const hubConnection = new HubConnectionBuilder()
        .withUrl(hubUrl)
        .build();

    const storage = new Storage(logger, hubConnection, []);

    configureHubEvents(hubConnection, storage);

    return {
        Storage: storage,
        Hub: hubConnection,
        Logger: logger
    };
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