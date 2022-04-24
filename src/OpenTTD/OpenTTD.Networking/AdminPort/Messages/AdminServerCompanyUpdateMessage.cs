using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerCompanyUpdateMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_COMPANY_UPDATE;

    public byte CompanyId { get; internal set; }

    public string CompanyName { get; internal set; }

    public string ManagerName { get; internal set; }

    public byte Color { get; internal set; }

    public bool HasPassword { get; internal set; }

    public byte MonthsOfBankruptcy { get; internal set; }

    public byte[] ShareOwnersIds { get; } = new byte[4];
}