using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public class AdminServerCompanyNewMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_COMPANY_NEW;

    public byte CompanyId { get; }

    public AdminServerCompanyNewMessage(byte companyId)
    {
        CompanyId = companyId;
    }
}