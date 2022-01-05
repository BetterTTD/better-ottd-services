import express from 'express';
import { connection } from 'node-openttd-admin';
import { v4 as uuidv4 } from 'uuid';
import { clientService } from './clientService.js';

// configure

const port = 80;
const app = express();
app.use(express.json());

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

    clientService.auth(client, ip, port, botName, pass);
    clientService.observe(client);

    let clientInfo = { id: uuidv4(), ip: ip, port: port, conn: client };
    clients = [...clients, clientInfo];

    res.status(200).send(clientInfo.id);
});

app.listen(port, () => console.log(`Example app listening at http://localhost:${port}`));