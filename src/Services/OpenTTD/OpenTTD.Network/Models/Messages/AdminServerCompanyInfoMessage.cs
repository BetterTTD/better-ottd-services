using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerCompanyInfoMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerCompanyInfo;

    public byte CompanyId { get; internal set; }

    public string CompanyName { get; internal set; }

    public string ManagerName { get; internal set; }

    public byte Color { get; internal set; }

    public bool HasPassword { get; internal set; }

    /// <summary>
    /// Defines when company was created.
    /// </summary>
    public OttdDate CreationDate { get; internal set; }

    public bool IsAi { get; internal set; }

    /// <summary>
    /// I do not know what this value represents. I will not need it right now but if there is a nice soul that would like to leave better comment - please do! :)
    /// </summary>
    public byte MonthsOfBankruptcy { get; internal set; }

    public byte[] ShareOwnersIds { get; } = new byte[4];
}