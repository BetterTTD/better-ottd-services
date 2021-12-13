using OpenTTD.Network.Models;
using OpenTTD.Network.Models.Enums;
using OpenTTD.Network.Models.Messages;

namespace OpenTTD.Network.AdminMappers;

public class AdminPacketService : IAdminPacketService
{
    public Packet CreatePacket(IAdminMessage message)
    {
        var packet = new Packet();
        packet.SendByte((byte)message.MessageType);

        switch (message.MessageType)
        {
            case AdminMessageType.AdminPacketAdminJoin:
            {
                var msg = message as AdminJoinMessage;
                packet.SendString(msg!.Password, 33);
                packet.SendString(msg.AdminName, 25);
                packet.SendString(msg.AdminVersion, 33);
                break;
            }

            case AdminMessageType.AdminPacketAdminPoll:
            {
                var msg = message as AdminPollMessage;
                packet.SendByte((byte)msg!.UpdateType);
                packet.SendU32(msg.Argument);

                break;
            }

            case AdminMessageType.AdminPacketAdminUpdateFrequency:
            {
                var msg = message as AdminUpdateFrequencyMessage;
                packet.SendU16((ushort)msg!.UpdateType);
                packet.SendU16((ushort)msg.UpdateFrequency);
                break;
            }

            case AdminMessageType.AdminPacketAdminChat:
            {
                var msg = message as AdminChatMessage;
                packet.SendByte((byte)msg!.NetworkAction);
                packet.SendByte((byte)msg.ChatDestination);
                packet.SendU32(msg.Destination);
                packet.SendString(msg.Message, 900);

                break;
            }

            case AdminMessageType.AdminPacketAdminRcon:
            {
                var msg = message as AdminRconMessage;
                packet.SendString(msg!.Command, 500);
                break;
            }

            case AdminMessageType.AdminPacketAdminPing:
            {
                var msg = message as AdminPingMessage;
                packet.SendU32(msg!.Argument);
                break;
            }

            case AdminMessageType.AdminPacketAdminQuit:
            case AdminMessageType.AdminPacketAdminGamescript: //this will be implemented later or never.
            {
                break;
            }

        }

        packet.PrepareToSend();
        return packet;
    }

    public IAdminMessage ReadPacket(Packet packet)
    {
        var type = (AdminMessageType)packet.ReadByte();
        switch (type)
        {
            case AdminMessageType.AdminPacketServerProtocol:
            {
                var dic = new Dictionary<AdminUpdateType, UpdateFrequency>();
                var version = packet.ReadByte();

                while (packet.ReadBool())
                {
                    var updateType = (AdminUpdateType)packet.ReadU16();
                    var frequency = packet.ReadU16();
                    dic.Add(updateType, (UpdateFrequency)frequency);
                }

                return new AdminServerProtocolMessage(version, dic);
            }
            case AdminMessageType.AdminPacketServerWelcome:
            {
                return new AdminServerWelcomeMessage
                {
                    ServerName = packet.ReadString(),
                    NetworkRevision = packet.ReadString(),
                    IsDedicated = packet.ReadBool(),
                    MapName = packet.ReadString(),
                    MapSeed = packet.ReadU32(),
                    Landscape = (Landscape)packet.ReadByte(),
                    CurrentDate = new OttdDate(packet.ReadU32()),
                    MapWidth = packet.ReadU16(),
                    MapHeight = packet.ReadU16(),
                };
            }
            case AdminMessageType.AdminPacketServerDate:
            {
                return new AdminServerDateMessage(packet.ReadU32());
            }
            case AdminMessageType.AdminPacketServerClientJoin:
            {
                return new AdminServerClientJoinMessage(packet.ReadU32());
            }
            case AdminMessageType.AdminPacketServerClientInfo:
            {
                return new AdminServerClientInfoMessage
                {
                    ClientId = packet.ReadU32(),
                    Hostname = packet.ReadString(),
                    ClientName = packet.ReadString(),
                    Language = packet.ReadByte(),
                    JoinDate = new OttdDate(packet.ReadU32()),
                    PlayingAs = packet.ReadByte(),
                };
            }
            case AdminMessageType.AdminPacketServerClientUpdate:
            {
                var clientId = packet.ReadU32();
                var clientName = packet.ReadString();
                var playingAs = packet.ReadByte();
                return new AdminServerClientUpdateMessage(clientId, clientName, playingAs);
            }

            case AdminMessageType.AdminPacketServerClientQuit:
            {
                return new AdminServerClientQuitMessage(packet.ReadU32());
            }
            case AdminMessageType.AdminPacketServerClientError:
            {
                return new AdminServerClientErrorMessage(packet.ReadU32(), packet.ReadByte());
            }
            case AdminMessageType.AdminPacketServerCompanyNew:
            {
                return new AdminServerCompanyNewMessage(packet.ReadByte());
            }
            case AdminMessageType.AdminPacketServerCompanyInfo:
            {
                var m = new AdminServerCompanyInfoMessage
                {
                    CompanyId = packet.ReadByte(),
                    CompanyName = packet.ReadString(),
                    ManagerName = packet.ReadString(),
                    Color = packet.ReadByte(),
                    HasPassword = packet.ReadBool(),
                    CreationDate = new OttdDate(packet.ReadU32()),
                    IsAi = packet.ReadBool(),
                    MonthsOfBankruptcy = packet.ReadByte()
                };
                for (var i = 0; i < m.ShareOwnersIds.Length; ++i) m.ShareOwnersIds[i] = packet.ReadByte();

                return m;
            }
            case AdminMessageType.AdminPacketServerCompanyUpdate:
            {
                var m = new AdminServerCompanyUpdateMessage
                {
                    CompanyId = packet.ReadByte(),
                    CompanyName = packet.ReadString(),
                    ManagerName = packet.ReadString(),
                    Color = packet.ReadByte(),
                    HasPassword = packet.ReadBool(),
                    MonthsOfBankruptcy = packet.ReadByte()
                };
                for (var i = 0; i < m.ShareOwnersIds.Length; ++i) m.ShareOwnersIds[i] = packet.ReadByte();

                return m;
            }
            case AdminMessageType.AdminPacketServerCompanyRemove:
            {
                return new AdminServerCompanyRemoveMessage(packet.ReadByte(), packet.ReadByte());
            }
            case AdminMessageType.AdminPacketServerCompanyEconomy:
            {
                var m = new AdminServerCompanyEconomyMessage
                {
                    CompanyId = packet.ReadByte(),
                    Money = packet.ReadU64(),
                    CurrentLoan = packet.ReadU64(),
                    Income = packet.ReadU64(),
                    DeliveredCargo = packet.ReadU16()
                };

                // TODO : add quarters.

                return m;
            }

            case AdminMessageType.AdminPacketServerChat:
            {
                var m = new AdminServerChatMessage
                {
                    NetworkAction = (NetworkAction)packet.ReadByte(),
                    ChatDestination = (ChatDestination)packet.ReadByte(),
                    ClientId = packet.ReadU32(),
                    Message = packet.ReadString(),
                    Data = packet.ReadI64()
                };

                return m;
            }

            case AdminMessageType.AdminPacketServerRcon:
            {
                return new AdminServerRconMessage(packet.ReadU16(), packet.ReadString());
            }

            case AdminMessageType.AdminPacketServerConsole:
            {
                return new AdminServerConsoleMessage(packet.ReadString(), packet.ReadString());
            }
            case AdminMessageType.AdminPacketServerPong:
            {
                return new AdminServerPongMessage(packet.ReadU32());
            }

            case AdminMessageType.AdminPacketServerNewgame:
            case AdminMessageType.AdminPacketServerShutdown:
            case AdminMessageType.AdminPacketServerCmdNames:
            case AdminMessageType.AdminPacketServerCmdLogging:
            case AdminMessageType.AdminPacketServerGamescript:
            case AdminMessageType.AdminPacketServerRconEnd:
            case AdminMessageType.AdminPacketServerCompanyStats: // I do not need this packet for now.
            {
                return new GenericAdminMessage(type);
            }
            default:
                throw new ArgumentException(type.ToString());
        }
    }
}