import express from 'express';
import { v4 as uuidv4 } from 'uuid';
import pinoToSeq from 'pino-seq';
import { pinoHttp } from 'pino-http';
import pinoms from 'pino-multi-stream';
import Server from './server.js';

// configure

const logger = pinoms({
    name: 'OpenTTD.AdminClient',
    streams: [
        { level: 'debug', stream: process.stdout },
        { level: 'error', stream: process.stderr },
        { level: 'debug', stream: pinoToSeq.createStream({ serverUrl: 'http://tg.seq:5341' })}
        //{ level: 'debug', stream: pinoToSeq.createStream({ serverUrl: 'http://localhost:5341' }) }
    ]
});

const port = 80;
const app = express();

app.use(express.json());
app.use(pinoHttp({
    logger: logger,
    wrapSerializers: true,
    useLevel: 'info',
    customSuccessMessage: function (res) {
        return `[${res.req.method}] ${res.req.url}`;
    }
}));

// app state

var servers = [];

// endpoints

app.get('/ping', (_, res) => {
    res.send('pong');
});

app.get('/servers', (_, res) => {
    res.status(200).send(servers.map(server => {
        return {
            id: server.id,
            ip: server.instance.info.ip,
            port: server.instance.info.port
        };
    }));
});

app.post('/servers', (req, res) => {
    let { ip, port, pass, botName } = req.body;

    if (servers.some(x => x.ip === ip && x.port === port)) {
        res.status(400).send('Server is already added.');
        return;
    }

    let server = new Server(logger, { ip, port, botName, pass });
    let id = uuidv4();
    servers = [...servers, { id: id, instance: server }];

    res.status(200).send(id);
});

app.put('/servers/:serverId/connect', (req, res) => {
    let { serverId } = req.params;
    let server = servers.find(s => s.id === serverId);
    if (server == null) {
        res.status(404).send('Server not found.');
        return;
    }
    server.instance.Connect();
    res.status(200).send();
});

app.delete('/servers/:serverId/disconnect', (req, res) => {
    let { serverId } = req.params;
    let server = servers.find(s => s.id === serverId);
    if (server == null) {
        res.status(404).send('Server not found.');
        return;
    }
    server.instance.Disconnect();
    res.status(200).send();
});


app.listen(port);