import express from 'express';
import { connection } from 'node-openttd-admin';
import { v4 as uuidv4 } from 'uuid';
import ClientService from './clientService.js';
import pinoToSeq from 'pino-seq';
import { pinoHttp } from 'pino-http';
import pinoms from 'pino-multi-stream';

// configure

//const stream = pinoToSeq.createStream({ serverUrl: 'http://localhost:5341' });

const logger = pinoms({
    name: 'OpenTTD.AdminClient', 
    streams: [
        {level: 'debug', stream: process.stdout},
        {level: 'error', stream: process.stderr},
        {level: 'debug', stream: pinoToSeq.createStream({ serverUrl: 'http://tg.seq:5341' })}
    ]
});

const clientService = new ClientService(logger);
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

var clients = [];

// endpoints

app.get('/', (_, res) => {
    res.send('Hello World!');
});

app.get('/ping', (_, res) => {
    res.send('pong');
});

app.get('/clients', (_, res) => {
    let result = clients.map(client => {
        return {
            id: client.id,
            ip: client.ip,
            port: client.port
        };
    });
    res.status(200).send(result);
});

app.post('/add-client', (req, res) => {
    let { ip, port, pass, botName } = req.body;

    if (clients.some(x => x.ip === ip && x.port === port)) {
        res.status(400).send('Server is already observed');
        return;
    }

    let client = new connection();

    clientService.Auth(client, ip, port, botName, pass);
    clientService.Observe(client);

    let clientInfo = { id: uuidv4(), ip: ip, port: port, conn: client };
    clients = [...clients, clientInfo];

    res.status(200).send(clientInfo.id);
});

app.listen(port);