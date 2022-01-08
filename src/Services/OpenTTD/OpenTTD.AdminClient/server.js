import { connection as Connection } from 'node-openttd-admin';
import packetEnums from 'node-openttd-admin/enums.js';

const { UpdateFrequencies, UpdateTypes } = packetEnums;

const rules = "Short server rules - No stealing, no trolling, no teleporting, no city grids.";

function sendRcon(connection, command) {
    connection.send_rcon(command);
}

function sayClient(connection, clientId, text) {
    sendRcon(connection, `say_client ${clientId} \"${text}\"`);
}

export default class Server {
    constructor(info, { Hub, Storage, Logger }) {
        this.info = info;
        this.Hub = Hub;
        this.Storage = Storage;
        this.Logger = Logger;
        this.clients = [];
        this.conn = new Connection();
        this._subscribe();
    }

    Connect() {
        this.conn.connect(this.info.ip, this.info.port);
    }

    Disconnect() {
        this.conn.close();
    }

    _subscribe() {
        this.conn.on('connect', this._connect);
        this.conn.on('welcome', this._welcome);
        this.conn.on('error', this._error);
        this.conn.on('chat', this._chat);
        this.conn.on('console', this._console);
        this.conn.on('clientinfo', this._clientInfo);
        this.conn.on('clientjoin', this._clientJoin);
        this.conn.on('clientupdate', this._clientUpdate);
        this.conn.on('clientquit', this._clientQuit);
        this.conn.on('clienterror', this._clientError);
    }

    _connect = () => {
        const { ip, port, name, pass } = this.info;
        this.Logger.info({ ip: ip, port: port }, '[CONNECT]');
        this.conn.authenticate(name, pass);
    };

    _welcome = (welcome) => {
        this.Logger.info(welcome, '[WELCOME]');
        this.conn.send_update_frequency(UpdateTypes.CHAT, UpdateFrequencies.AUTOMATIC);
        this.conn.send_update_frequency(UpdateTypes.CLIENT_INFO, UpdateFrequencies.AUTOMATIC);
        //client.send_update_frequency(UpdateTypes.CONSOLE, UpdateFrequencies.AUTOMATIC);
        this.conn.send_poll(UpdateTypes.CLIENT_INFO, Number.MAX_SAFE_INTEGER);
        
        const hubEvent = { 
            ip: this.info.ip, 
            port: this.info.port, 
            name: welcome.name, 
            version: welcome.version, 
            map: {
                seed: welcome.map.seed,
                landscape: welcome.map.landscape,
                height: welcome.map.height,
                width: welcome.map.width
            }
        };
        console.log(JSON.stringify(hubEvent));
        this.Hub.send('TellServerInfoUpdated', this.info.id, hubEvent).catch((err) => console.log(err));
    };

    _chat = (chat) => {
        this.Logger.info(chat, '[CHAT]');

        let sayCli = (message) => sayClient(this.conn, chat.id, message);
        let sendRc = (command) => sendRcon(this.conn, command);

        let cmdExact = (str) => chat.message === str;
        let cmdStarts = (str) => chat.message.startsWith(str);

        switch (true) {
            case cmdExact('!kek'):
                sayCli('cheburek!')
                break;
            case cmdExact('!rules'):
                sayCli(rules);
                break;
            case cmdExact('!reset') || cmdExact('!resetme'):
                let cli = this.clients.find(c => c.id === chat.id);
                let companyResetCmd = `reset_company ${cli.company}`;
                let moveClientCmd = `move ${cli.id} 255`;
                sendRc(moveClientCmd);
                sendRc(companyResetCmd);
                break;
            case cmdStarts('!rename ') || cmdStarts('!name '):
                if (chat.message.toLowerCase().includes('admin')) {
                    sayCli('Forbidden name! Admin isn\'t allowed.');
                    break;
                }
                let clientNameCmd = `client_name ${chat.id} \"${chat.message.replace('!rename ', '').replace('!name ', '')}\"`;
                sendRc(clientNameCmd);
                break;
            default:
                break;
        }
    }

    _clientJoin = (clientJoin) => {
        this.Logger.info(clientJoin, '[CLIENTJOIN]');

        let sayCli = (message) => sayClient(this.conn, clientJoin, message);
        sayCli("Welcome to TG Vanilla server - reddit.com/r/openttd legacy.");
        sayCli(rules);
        sayCli("Server commands:");
        sayCli("!rules");
        sayCli("!rename or !name");
        sayCli("!resetme or !reset");
        sayCli("!info");
    }

    _error = (error) => {
        this.Logger.info(error, '[ERROR]');
        if (error === 'connectionerror') {
            this.Disconnect();
            setTimeout(() => this.Connect(), 5000);
        }
    }

    _console = (consol) => {
        this.Logger.info(consol, '[CONSOLE]');
    }

    _clientInfo = (clientInfo) => {
        this.Logger.info(clientInfo, '[CLIENT_INFO]');
        this.clients = [...this.clients.filter(c => c.id !== clientInfo.id), { ...clientInfo, company: clientInfo.company + 1 }];
    }

    _clientUpdate = (clientUpdate) => {
        this.Logger.info(clientUpdate, '[CLIENTUPDATE]');
        let cli = this.clients.find(c => c.id === clientUpdate.id);
        let updatedProps = { name: clientUpdate.name, company: clientUpdate.company + 1 };
        this.clients = [...this.clients.filter(c => c.id !== clientUpdate.id), { ...cli, ...updatedProps }];
        if (clientUpdate.name.toLowerCase().includes('admin')) {
            sendRcon(this.conn, `say_client ${cli.id} \"Forbidden name! Admin isn\'t allowed.\"`);
            sendRcon(this.conn, `client_name ${cli.id} \"${cli.name}\"`);
        }
    }

    _clientQuit = (clientQuit) => {
        this.Logger.info(clientQuit, '[CLIENTQUIT]');
        this.clients = [...this.clients.filter(c => c.id !== clientQuit.id)];
    }

    _clientError = (clientError) => {
        this.Logger.info(clientError, '[CLIENTERROR]');
        this.clients = [...this.clients.filter(c => c.id !== clientError.id)];
    }
}