import { v4 as uuidv4 } from 'uuid';

let onGetServers = ({ Storage }) => (_req, res) => {
    res.status(200).send(Storage.state.map(server => {
        return {
            id: server.id,
            ip: server.instance.info.ip,
            port: server.instance.info.port
        };
    }));
};

let onAddServer = ({ Storage }) => (req, res) => {
    const { ip, port } = req.body;
    const id = uuidv4();

    if (Storage.state.some(server => server.instance.ip === ip && server.instance.port === port)) {
        res.status(400).send('Server is already added.');
        return;
    }

    Storage.dispatch('SERVER_CREATE', { id, services, ...req.body });
    res.status(200).send(id);
};

let onServerConnect = ({ Storage }) => (req, res) => {
    let { serverId } = req.params;
    if (!Storage.state.some(s => s.id === serverId)) {
        res.status(404).send('Server not found.');
        return;
    }

    Storage.dispatch('SERVER_CONNECT', { serverId });
    res.status(200).send();
};

let onServerDisconnect = ({ Storage }) => (req, res) => {
    let { serverId } = req.params;
    if (!Storage.state.some(s => s.id === serverId)) {
        res.status(404).send('Server not found.');
        return;
    }
    Storage.dispatch('SERVER_DISCONNECT', { serverId });
    res.status(200).send();
};

export default function configureAppRoutes(app, services) {
    app.get('/ping', (_, res) => res.send('pong'));
    app.get('/servers', onGetServers(services));
    app.post('/servers', onAddServer(services));
    app.put('/servers/:serverId/connect', onServerConnect(services));
    app.put('/servers/:serverId/disconnect', onServerDisconnect(services));
};