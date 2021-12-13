using OpenTTD.Network.AdminPort.Events;
using OpenTTD.Network.AdminPort.Messages;
using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort;

public class AdminMessageProcessor : IAdminMessageProcessor
{
    public IAdminEvent ProcessMessage(IAdminMessage adminMessage, in IAdminPortClient client)
    {
        switch (adminMessage.MessageType)
        {
            case AdminMessageType.AdminPacketServerChat:
            {
                var msg = adminMessage as AdminServerChatMessage;
                if (msg.NetworkAction != NetworkAction.NETWORK_ACTION_SERVER_MESSAGE)
                    return null;
                var player = client.Players[msg.ClientId];

                return new AdminChatMessageEvent(player, msg.Message, client.ServerInfo);
            }
            case AdminMessageType.AdminPacketServerConsole:
            {
                var msg = adminMessage as AdminServerConsoleMessage;

                return new AdminConsoleEvent(client.ServerInfo, msg.Origin, msg.Message);
            }
            case AdminMessageType.AdminPacketServerRcon:
            {
                var msg = adminMessage as AdminServerRconMessage;

                return new AdminRconEvent(client.ServerInfo, msg.Result);
            }
            case AdminMessageType.AdminPacketServerPong:
            {
                var msg = adminMessage as AdminServerPongMessage;

                return new AdminPongEvent(client.ServerInfo, msg.Argument);
            }
            default:
                return null;
        }
    }
}