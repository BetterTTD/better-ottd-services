using Networking.Enums;

namespace Networking.Messages.Inbound.ServerCompanyRemove;

public sealed record ServerCompanyRemoveMessage(
    byte CompanyId, 
    CompanyRemoveReason RemoveReason) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_REMOVE;
}