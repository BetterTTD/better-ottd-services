using Microsoft.Extensions.Logging;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort;

public class AdminPortClientFactory : IAdminPortClientFactory
{
    private readonly IAdminPacketService _adminPacketService;
    private readonly ILogger<IAdminPortClient> _logger;
    private readonly IAdminMessageProcessor _messageProcessor;

    public AdminPortClientFactory(
        IAdminPacketService adminPacketService, 
        IAdminMessageProcessor messageProcessor, 
        ILogger<IAdminPortClient> logger)
    {
        _adminPacketService = adminPacketService;
        _logger = logger;
        _messageProcessor = messageProcessor;
    }

    public virtual IAdminPortClient Create(ServerInfo info)
    {
        return new AdminPortClient(info, _adminPacketService, _messageProcessor, _logger);
    }
}