using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerCompanyNew;

public class AdminServerCompanyNewMessage : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_COMPANY_NEW;

    public byte CompanyId { get; }

    public AdminServerCompanyNewMessage(byte companyId)
    {
        CompanyId = companyId;
    }
}