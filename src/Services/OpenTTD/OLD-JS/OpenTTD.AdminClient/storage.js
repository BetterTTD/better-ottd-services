import Server from "./server.js";

function serverCreate(storage, { id, services, ip, port, botName, pass }) {
    if (storage.state.some(server => server.instance.ip === ip && server.instance.port === port)) {
        throw 'Domain exception';
    }

    let instance = new Server({ id, ip, port, botName, pass }, services);
    let server = { id: id, instance: instance };
    storage.state = [...storage.state, server];

    let hubEvent = { ip: ip, port: port, name: null, version: null, map: null };
    Hub.send('TellServerInfoUpdated', id, hubEvent)
       .catch((err) => console.log(err));
}

function serverConnect(storage, { serverId }) {
    let server = storage.state.find(s => s.id === serverId);
    server.instance.Connect();
}

function serverDisconnect(storage, { serverId }) {
    let server = storage.state.find(s => s.id === serverId);
    server.instance.Disconnect();
}

export default class Storage {
    constructor(logger, hub, initial) {
        this.logger = logger;
        this.hub = hub;
        this.state = initial;
    }

    dispatch(msg, param) {
        switch (msg) {
            case 'SERVER_CREATE':
                serverCreate(this, param);
                break;

            case 'SERVER_CONNECT':
                serverConnect(this, param);
                break;

            case 'SERVER_DISCONNECT':
                serverDisconnect(this, param);
                break;
        }
        return this.state;
    }
}