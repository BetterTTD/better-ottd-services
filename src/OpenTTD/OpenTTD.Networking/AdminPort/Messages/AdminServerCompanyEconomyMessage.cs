using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerCompanyEconomyMessage(
    byte CompanyId,
    ulong Money,
    ulong CurrentLoan,
    ulong Income,
    ushort DeliveredCargo) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_COMPANY_ECONOMY;
}