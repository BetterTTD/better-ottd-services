using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerCompanyRemove;

public sealed record AdminServerCompanyRemoveMessage(
    byte CompanyId, 
    AdminCompanyRemoveReason RemoveReason) : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_COMPANY_REMOVE;
}