using OpenTTD.Network.AdminClient;
using OpenTTD.Network.Models.Events;
using OpenTTD.Network.Models.Messages;

namespace OpenTTD.Network.AdminMappers;

public interface IAdminMessageProcessor
{
    IAdminEvent ProcessMessage(IAdminMessage adminMessage, in IAdminPortClient client);
        
}