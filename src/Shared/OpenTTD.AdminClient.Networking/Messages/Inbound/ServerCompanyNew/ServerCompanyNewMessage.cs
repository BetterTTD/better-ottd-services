using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerCompanyNew;

public class ServerCompanyNewMessage(byte companyId) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_NEW;

    public byte CompanyId { get; } = companyId;
}