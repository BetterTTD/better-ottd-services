using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerCompanyNew;

public class ServerCompanyNewMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_NEW;

    public byte CompanyId { get; }

    public ServerCompanyNewMessage(byte companyId)
    {
        CompanyId = companyId;
    }
}