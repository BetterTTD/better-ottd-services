using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerCompanyRemoveMessage(
    byte CompanyId, 
    AdminCompanyRemoveReason RemoveReason) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_COMPANY_REMOVE;
}