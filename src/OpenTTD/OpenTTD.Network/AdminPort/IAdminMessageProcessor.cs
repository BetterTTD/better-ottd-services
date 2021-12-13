using OpenTTD.Network.AdminPort.Events;
using OpenTTD.Network.AdminPort.Messages;

namespace OpenTTD.Network.AdminPort;

public interface IAdminMessageProcessor
{
    IAdminEvent ProcessMessage(IAdminMessage adminMessage, in IAdminPortClient client);
        
}