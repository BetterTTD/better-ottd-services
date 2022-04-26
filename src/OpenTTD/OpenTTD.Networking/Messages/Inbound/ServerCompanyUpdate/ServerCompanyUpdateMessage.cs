using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerCompanyUpdate;

public sealed record ServerCompanyUpdateMessage : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_UPDATE;

    public byte CompanyId { get; init; }
    public string CompanyName { get; init; }
    public string ManagerName { get; init; }
    public byte Color { get; init; }
    public bool HasPassword { get; init; }
    public byte MonthsOfBankruptcy { get; init; }
    public byte[] ShareOwnersIds { get; } = new byte[4];
}