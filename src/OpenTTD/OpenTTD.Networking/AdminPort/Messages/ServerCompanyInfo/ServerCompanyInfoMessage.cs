using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerCompanyInfo;

public sealed record ServerCompanyInfoMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_INFO;

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