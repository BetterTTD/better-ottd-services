using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerCompanyUpdateMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerCompanyUpdate;

    public byte CompanyId { get; internal set; }

    public string CompanyName { get; internal set; }

    public string ManagerName { get; internal set; }

    public byte Color { get; internal set; }

    public bool HasPassword { get; internal set; }

    public byte MonthsOfBankruptcy { get; internal set; }

    public byte[] ShareOwnersIds { get; } = new byte[4];
}