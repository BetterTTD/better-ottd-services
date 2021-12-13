using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminServerCompanyNewMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerCompanyNew;

    public byte CompanyId { get; }

    public AdminServerCompanyNewMessage(byte companyId)
    {
        CompanyId = companyId;
    }
}