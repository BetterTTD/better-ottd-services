namespace OpenTTD.API.Network.AdminPort;

public interface IAdminPortClientFactory
{
    IAdminPortClient Create(ServerInfo serverInfo);
        
}