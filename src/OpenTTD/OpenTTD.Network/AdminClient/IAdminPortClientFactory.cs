using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminClient;

public interface IAdminPortClientFactory
{
    IAdminPortClient Create(ServerInfo serverInfo);
        
}