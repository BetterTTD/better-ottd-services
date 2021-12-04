namespace OpenTTD.API.Network.AdminPort;

public class AdminServerCompanyRemoveMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerCompanyRemove;

    public byte CompanyId { get; }

    public AdminCompanyRemoveReason RemoveReason { get; }

    public AdminServerCompanyRemoveMessage(byte companyId, byte removeReason)
    {
        CompanyId = companyId;
        RemoveReason = (AdminCompanyRemoveReason) removeReason;
    }

}