using System.Collections.Concurrent;
using System.Net.Sockets;

namespace OpenTTD.API.Network.AdminPort;

public class AdminPortClient : IAdminPortClient
{
    private TcpClient _tcpClient;
    public AdminConnectionState ConnectionState { get; private set; }

    public ConcurrentDictionary<uint, Player> Players { get; } = new();

    public event EventHandler<IAdminEvent> EventReceived;

    private readonly ILogger _logger;

    private readonly IAdminPacketService _adminPacketService;

    private readonly IAdminMessageProcessor _messageProcessor;

    private readonly ConcurrentQueue<IAdminMessage> _receivedMessagesQueue = new();

    private readonly ConcurrentQueue<IAdminMessage> _sendMessageQueue = new();

    private DateTime _lastMessageSentTime = DateTime.Now;
    private DateTime _lastMessageReceivedTime = DateTime.Now;

    private Mutex _startMutex = new();



    private CancellationTokenSource _cancellationTokenSource = null;

    public ServerInfo ServerInfo { get; }

    public ConcurrentDictionary<AdminUpdateType, AdminUpdateSetting> AdminUpdateSettings { get; } = new();


    public AdminServerInfo AdminServerInfo { get; private set; } = new();

    public AdminPortClient(ServerInfo serverInfo, IAdminPacketService adminPacketService,
        IAdminMessageProcessor messageProcessor, ILogger<IAdminPortClient> logger)
    {
        ServerInfo = serverInfo;
        this._logger = logger;
        this._adminPacketService = adminPacketService;
        this._messageProcessor = messageProcessor;

        foreach (var type in Enums.ToArray<AdminUpdateType>())
        {
            AdminUpdateSettings.TryAdd(type,
                new AdminUpdateSetting(false, type, UpdateFrequency.AdminFrequencyAutomatic));
        }
    }

    private async void EventLoop(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            if (_receivedMessagesQueue.TryDequeue(out var msg))
            {
                var eventMessage = _messageProcessor.ProcessMessage(msg, this);

                if (eventMessage != null)
                    EventReceived?.Invoke(this, eventMessage);
            }

            await Task.Delay(TimeSpan.FromSeconds(0.1), token);
        }

    }

    private async void MainLoop(CancellationToken token)
    {
        Task<int> sizeTask = null;
        var sizeBuffer = new byte[2];

        while (token.IsCancellationRequested == false)
        {
            try
            {
                if (ConnectionState == AdminConnectionState.NotConnected)
                {
                    _tcpClient = new TcpClient();
                    _tcpClient.ReceiveTimeout = 2000;
                    _tcpClient.SendTimeout = 2000;
                    _lastMessageSentTime = DateTime.Now;
                    _lastMessageReceivedTime = DateTime.Now;
                    _tcpClient.Connect(ServerInfo.ServerIp, ServerInfo.ServerPort);
                    SendMessage(new AdminJoinMessage(ServerInfo.Password, "OttdBot", "1.0.0"));
                    _logger.LogInformation($"{ServerInfo} Connecting");

                    ConnectionState = AdminConnectionState.Connecting;
                }

                if (_tcpClient == null)
                    continue;

                if ((DateTime.Now - _lastMessageSentTime) > TimeSpan.FromSeconds(10))
                {
                    SendMessage(new AdminPingMessage());
                }

                if (DateTime.Now - _lastMessageReceivedTime > TimeSpan.FromMinutes(1))
                {
                    throw new OttdException("No messages received for 60 seconds!");
                }

                for (var i = 0; i < 100; ++i)
                {
                    if (_sendMessageQueue.TryDequeue(out var msg))
                    {
                        _logger.LogInformation($"{ServerInfo} sent {msg.MessageType}");
                        var packet = _adminPacketService.CreatePacket(msg);
                        await _tcpClient.GetStream().WriteAsync(packet.Buffer, 0, packet.Size)
                            .WaitMax(TimeSpan.FromSeconds(2));
                        _lastMessageSentTime = DateTime.Now;
                    }
                    else
                        break;
                }

                while ((sizeTask ??= _tcpClient.GetStream().ReadAsync(sizeBuffer, 0, 2)).IsCompleted)
                {
                    var receivedBytes = sizeTask.Result;

                    if (receivedBytes != 2)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(1));
                        int bytes = await _tcpClient.GetStream().ReadAsync(sizeBuffer, 1, 1)
                            .WaitMax(TimeSpan.FromSeconds(2));
                        if (bytes == 0)
                        {
                            throw new OttdConnectionException("Something went wrong - restarting");
                        }

                    }

                    sizeTask = null;

                    var size = BitConverter.ToUInt16(sizeBuffer, 0);

                    var content = new byte[size];
                    content[0] = sizeBuffer[0];
                    content[1] = sizeBuffer[1];
                    var contentSize = 2;

                    _lastMessageReceivedTime = DateTime.Now;

                    do
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(1));
                        Task<int> task = _tcpClient.GetStream().ReadAsync(content, contentSize, size - contentSize)
                            .WaitMax(TimeSpan.FromSeconds(2), $"{ServerInfo} no data received");
                        await task;
                        contentSize += task.Result;
                        if (task.Result == 0)
                        {
                            throw new OttdConnectionException("No further data received in message!");
                        }
                    } while (contentSize < size);


                    var packet = new Packet(content);
                    var message = _adminPacketService.ReadPacket(packet);
                    if (message == null)
                        break;

                    _logger.LogInformation($"{ServerInfo} received {message.MessageType}");

                    switch (message.MessageType)
                    {
                        case AdminMessageType.AdminPacketServerProtocol:
                        {
                            var msg = message as AdminServerProtocolMessage;

                            foreach (var s in msg.AdminUpdateSettings)
                            {
                                _logger.LogInformation($"Update settings {s.Key} - {s.Value}");
                                AdminUpdateSettings.TryUpdate(s.Key, new AdminUpdateSetting(true, s.Key, s.Value),
                                    AdminUpdateSettings[s.Key]);
                            }

                            break;
                        }
                        case AdminMessageType.AdminPacketServerWelcome:
                        {
                            var msg = message as AdminServerWelcomeMessage;

                            AdminServerInfo = new AdminServerInfo()
                            {
                                IsDedicated = msg.IsDedicated,
                                MapName = msg.MapName,
                                RevisionName = msg.NetworkRevision,
                                ServerName = msg.ServerName
                            };


                            SendMessage(new AdminUpdateFrequencyMessage(AdminUpdateType.AdminUpdateChat,
                                UpdateFrequency.AdminFrequencyAutomatic));
                            SendMessage(new AdminUpdateFrequencyMessage(AdminUpdateType.AdminUpdateConsole,
                                UpdateFrequency.AdminFrequencyAutomatic));
                            SendMessage(new AdminUpdateFrequencyMessage(AdminUpdateType.AdminUpdateClientInfo,
                                UpdateFrequency.AdminFrequencyAutomatic));
                            SendMessage(new AdminPollMessage(AdminUpdateType.AdminUpdateClientInfo,
                                uint.MaxValue));

                            ConnectionState = AdminConnectionState.Connected;
                            _logger.LogInformation($"{ServerInfo.ServerIp}:{ServerInfo.ServerPort} - connected");

                            break;
                        }
                        case AdminMessageType.AdminPacketServerClientInfo:
                        {
                            var msg = message as AdminServerClientInfoMessage;
                            var player = new Player(msg.ClientId, msg.ClientName);
                            Players.AddOrUpdate(msg.ClientId, player, (_, __) => player);

                            break;
                        }
                        case AdminMessageType.AdminPacketServerClientUpdate:
                        {
                            var msg = message as AdminServerClientUpdateMessage;
                            var player = Players[msg.ClientId];
                            player.Name = msg.ClientName;

                            break;
                        }
                        default:
                        {
                            var msg = message as AdminServerChatMessage;
                            _receivedMessagesQueue.Enqueue(message);
                            break;
                        }
                    }

                }



                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }
            catch (Exception e)
            {
                _logger.LogError($"{ServerInfo.ServerIp}:{ServerInfo.ServerPort} encountered error {e.Message}", e);

                _tcpClient?.Dispose();
                _tcpClient = null;
                _sendMessageQueue.Clear();
                _receivedMessagesQueue.Clear();
                sizeTask = null;
                ConnectionState = AdminConnectionState.NotConnected;

                await Task.Delay(TimeSpan.FromSeconds(60));
            }

        }

        _logger.LogInformation($"{ServerInfo} disconnected");
        ConnectionState = AdminConnectionState.Idle;
    }

    public async Task Join()
    {
        if (ConnectionState != AdminConnectionState.Idle)
            return;

        lock (_startMutex)
        {
            if (ConnectionState == AdminConnectionState.Idle)
            {
                ConnectionState = AdminConnectionState.NotConnected;
            }
            else
            {
                return;
            }
        }

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();

            ThreadPool.QueueUserWorkItem(_ => MainLoop(_cancellationTokenSource.Token), null);
            ThreadPool.QueueUserWorkItem(_ => EventLoop(_cancellationTokenSource.Token), null);

            if (!(await TaskHelper.WaitUntil(() => ConnectionState == AdminConnectionState.Connected,
                    delayBetweenChecks: TimeSpan.FromSeconds(0.5), duration: TimeSpan.FromSeconds(10))))
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
                throw new OttdConnectionException("Admin port could not connect to the server");
            }
        }
        catch (Exception e)
        {
            ConnectionState = AdminConnectionState.Idle;
            throw new OttdConnectionException("Could not join server", e);
        }
    }

    public async Task Disconnect()
    {
        if (ConnectionState == AdminConnectionState.Idle)
            return;
        try
        {
            _cancellationTokenSource.Cancel();

            if (!(await TaskHelper.WaitUntil(() => ConnectionState == AdminConnectionState.Idle,
                    delayBetweenChecks: TimeSpan.FromSeconds(0.5), duration: TimeSpan.FromSeconds(10))))
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }
        }
        catch (Exception e)
        {
            throw new OttdConnectionException("Error during stopping server", e);
        }
    }

    public void SendMessage(IAdminMessage message)
    {
        _sendMessageQueue.Enqueue(message);
    }
}