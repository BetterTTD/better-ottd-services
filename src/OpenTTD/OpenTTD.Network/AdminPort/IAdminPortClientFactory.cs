using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort;

public interface IAdminPortClientFactory
{
    IAdminPortClient Create(ServerInfo serverInfo);
        
}