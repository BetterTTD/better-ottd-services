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
    constructor(logger, info) {
        this.logger = logger;
        this.info = info;
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
        let { ip, port, name, pass } = this.info;
        this.logger.info({ ip: ip, port: port }, '[CONNECT]');
        this.conn.authenticate(name, pass);
    };

    _welcome = (welcome) => {
        this.logger.info(welcome, '[WELCOME]');
        this.conn.send_update_frequency(UpdateTypes.CHAT, UpdateFrequencies.AUTOMATIC);
        this.conn.send_update_frequency(UpdateTypes.CLIENT_INFO, UpdateFrequencies.AUTOMATIC);
        //client.send_update_frequency(UpdateTypes.CONSOLE, UpdateFrequencies.AUTOMATIC);
        this.conn.send_poll(UpdateTypes.CLIENT_INFO, Number.MAX_SAFE_INTEGER);
    };

    _chat = (chat) => {
        this.logger.info(chat, '[CHAT]');

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
                let clientNameCmd = `client_name ${chat.id} \"${chat.message.replace('!rename ', '').replace('!name ', '')}\"`;
                sendRc(clientNameCmd);
                break;
            default:
                break;
        }
    }

    _clientJoin = (clientJoin) => {
        this.logger.info(clientJoin, '[CLIENTJOIN]');

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
        this.logger.info(error, '[ERROR]');
        if (error === 'connectionerror') {
            this.Disconnect();
            setTimeout(() => this.Connect(), 5000);
        }
    }

    _console = (consol) => {
        this.logger.info(consol, '[CONSOLE]');
    }

    _clientInfo = (clientInfo) => {
        this.logger.info(clientInfo, '[CLIENT_INFO]');
        this.clients = [...this.clients.filter(c => c.id !== clientInfo.id), { ...clientInfo, company: clientInfo.company + 1 }];
    }

    _clientUpdate = (clientUpdate) => {
        this.logger.info(clientUpdate, '[CLIENTUPDATE]');
        let cli = this.clients.find(c => c.id === clientUpdate.id);
        let updatedProps = { name: clientUpdate.name, company: clientUpdate.company + 1 };
        this.clients = [...this.clients.filter(c => c.id !== clientUpdate.id), { ...cli, ...updatedProps }];
    }

    _clientQuit = (clientQuit) => {
        this.logger.info(clientQuit, '[CLIENTQUIT]');
        this.clients = [...this.clients.filter(c => c.id !== clientQuit.id)];
    }

    _clientError = (clientError) => {
        this.logger.info(clientError, '[CLIENTERROR]');
        this.clients = [...this.clients.filter(c => c.id !== clientError.id)];
    }
}