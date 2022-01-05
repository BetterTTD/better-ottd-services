import packetEnums from 'node-openttd-admin/enums.js';

const { UpdateFrequencies, UpdateTypes } = packetEnums;

const rules = "Short server rules - No stealing, no trolling, no teleporting, no city grids.";

let clients = [];

function clientAuth(client, ip, port, name, pass) {
    client.connect(ip, port);
    client.on('connect', () => {
        console.log(`[CONNECT]:`);
        client.authenticate(name, pass);
    });
}

function sendRcon(client, command) {
    client.send_rcon(command);
}

function sayClient(client, clientId, text) {
    sendRcon(client, `say_client ${clientId} \"${text}\"`);
}

function clientObserve(client) {
    client.on('welcome', (welcome) => {
        console.log(`[WELCOME]: ${JSON.stringify(welcome)}`);
        client.send_update_frequency(UpdateTypes.CHAT, UpdateFrequencies.AUTOMATIC);
        client.send_update_frequency(UpdateTypes.CLIENT_INFO, UpdateFrequencies.AUTOMATIC);
        //client.send_update_frequency(UpdateTypes.CONSOLE, UpdateFrequencies.AUTOMATIC);
        client.send_poll(UpdateTypes.CLIENT_INFO, Number.MAX_SAFE_INTEGER);
    });

    client.on('chat', (chat) => {
        console.log(`[CHAT]: ${JSON.stringify(chat)}`);

        let sayCli = (message) => sayClient(client, chat.id, message);
        let sendRc = (command) => sendRcon(client, command);

        let cmdExact = (str) => chat.message === str;
        let cmdStarts = (str) => chat.message.startsWith(str);

        switch (true) {
            case cmdExact('!rules'):
                sayCli(rules);
                break;
            case cmdExact('!reset') || cmdExact('!resetme'):
                let cli = clients.find(c => c.id === chat.id);
                let companyResetCmd = `reset_company ${cli.company}`;
                let moveClientCmd = `move ${cli.id} 255`;
                sendRc(moveClientCmd);
                sendRc(companyResetCmd);
                break;
            case cmdStarts('!rename ') ||cmdStarts('!name '):
                let clientNameCmd = `client_name ${chat.id} \"${chat.message.replace('!rename ', '').replace('!name ', '')}\"`;
                sendRc(clientNameCmd);
                break;
            default:
                break;
        }
    });

    client.on('clientinfo', (clientInfo) => {
        console.log(`[CLIENT_INFO]: ${JSON.stringify(clientInfo)}`);
        clients = [...clients.filter(c => c.id !== clientInfo.id), { ...clientInfo, company: clientInfo.company + 1 }];
    });

    client.on('error', function (error) {
        console.log(`[ERROR]: ${JSON.stringify(error)}`);
        clients = [...clients.filter(c => c.id !== error.id)];
    });

    client.on('clientjoin', function (clientJoin) {
        console.log(`[CLIENTJOIN]: ${JSON.stringify(clientJoin)}`);

        let sayCli = (message) => sayClient(client, clientJoin, message);
        sayCli("Welcome to TG Vanilla server - reddit.com/r/openttd legacy.");
        sayCli(rules);
        sayCli("Server commands:");
        sayCli("!rules");
        sayCli("!rename or !name");
        sayCli("!resetme or !reset");
        sayCli("!info");
    });

    client.on('clientupdate', function (clientUpdate) {
        console.log(`[CLIENTUPDATE]: ${JSON.stringify(clientUpdate)}`);
        let cli = clients.find(c => c.id === clientUpdate.id);
        let updatedProps = { name: clientUpdate.name, company: clientUpdate.company + 1 };
        clients = [...clients.filter(c => c.id !== clientUpdate.id), { ...cli, ...updatedProps }];
    });

    client.on('clientquit', function (clientQuit) {
        console.log(`[CLIENTQUIT]: ${JSON.stringify(clientQuit)}`);
        clients = [...clients.filter(c => c.id !== clientQuit.id)];
    });

    client.on('clienterror', function (clientError) {
        console.log(`[CLIENTERROR]: ${JSON.stringify(clientError)}`);
        clients = [...clients.filter(c => c.id !== clientError.id)];
    });

    client.on('console', function (consol) {
        console.log(`[CONSOLE]: ${JSON.stringify(consol)}`);
    });
}

export let clientService = {
    auth: clientAuth,
    observe: clientObserve
};