using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerCompanyNewMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerCompanyNew;

    public byte CompanyId { get; }

    public AdminServerCompanyNewMessage(byte companyId)
    {
        CompanyId = companyId;
    }
}