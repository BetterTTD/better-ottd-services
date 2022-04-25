using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerCompanyInfo;

public sealed record AdminServerCompanyInfoMessage : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_COMPANY_INFO;

    public byte CompanyId { get; init; }
    public string CompanyName { get; init; }
    public string ManagerName { get; init; }
    public byte Color { get; init; }
    public bool HasPassword { get; init; }
    public long CreationDate { get; init; }
    public bool IsAi { get; init; }
    public byte MonthsOfBankruptcy { get; init; }
    public byte[] ShareOwnersIds { get; } = new byte[4];
}