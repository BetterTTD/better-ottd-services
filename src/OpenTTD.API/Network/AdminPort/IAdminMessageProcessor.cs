namespace OpenTTD.API.Network.AdminPort;

public interface IAdminMessageProcessor
{
    IAdminEvent ProcessMessage(IAdminMessage adminMessage, in IAdminPortClient client);
        
}