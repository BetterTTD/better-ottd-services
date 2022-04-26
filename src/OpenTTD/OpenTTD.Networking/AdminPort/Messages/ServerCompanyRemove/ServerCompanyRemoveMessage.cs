using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerCompanyRemove;

public sealed record ServerCompanyRemoveMessage(
    byte CompanyId, 
    AdminCompanyRemoveReason RemoveReason) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_REMOVE;
}