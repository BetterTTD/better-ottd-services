using OpenTTD.Network.Enums;

namespace OpenTTD.Network.Models;

public abstract record AdminMessage(AdminMessageType MessageType);

public record GenericAdminMessage(AdminMessageType MessageType) : AdminMessage(MessageType);

#region Admin

public record AdminUpdateFrequencyMessage(
        AdminUpdateType UpdateType,
        UpdateFrequency UpdateFrequency)
    : AdminMessage(AdminMessageType.AdminPacketAdminUpdateFrequency)
{
    public override string ToString() => $"Update FREQ {UpdateType} to {UpdateFrequency}";
};

public record AdminChatMessage(
        NetworkAction NetworkAction,
        ChatDestination ChatDestination,
        uint Destination,
        string Message)
    : AdminMessage(AdminMessageType.AdminPacketAdminChat);

public record AdminPingMessage(
        uint Argument = 0)
    : AdminMessage(AdminMessageType.AdminPacketAdminPing);

public record AdminPollMessage(
        AdminUpdateType UpdateType,
        uint Argument)
    : AdminMessage(AdminMessageType.AdminPacketAdminPoll);

public record AdminRconMessage(
        string Command)
    : AdminMessage(AdminMessageType.AdminPacketAdminRcon);

#endregion

#region AdminServer

public record AdminJoinMessage(
        string Password,
        string AdminName,
        string AdminVersion)
    : AdminMessage(AdminMessageType.AdminPacketAdminJoin);

public record AdminServerWelcomeMessage(
        string ServerName,
        string NetworkRevision,
        bool IsDedicated,
        string MapName,
        uint MapSeed,
        Landscape Landscape,
        OttdDate CurrentDate,
        ushort MapWidth,
        ushort MapHeight)
    : AdminMessage(AdminMessageType.AdminPacketServerWelcome);

public record AdminServerProtocolMessage(
        byte NetworkVersion,
        Dictionary<AdminUpdateType, UpdateFrequency> AdminUpdateSettings)
    : AdminMessage(AdminMessageType.AdminPacketServerProtocol);

public record AdminServerRconMessage(
        ushort Color,
        string Result)
    : AdminMessage(AdminMessageType.AdminPacketServerRcon);

public record AdminServerPongMessage(
        uint Arg)
    : AdminMessage(AdminMessageType.AdminPacketServerPong);

public record AdminServerDateMessage
    : AdminMessage
{
    public OttdDate Date { get; }

    public AdminServerDateMessage(uint date)
        : base(AdminMessageType.AdminPacketServerDate) =>
        Date = new OttdDate(date);
};

public record AdminServerConsoleMessage(
        string Origin,
        string Message)
    : AdminMessage(AdminMessageType.AdminPacketServerConsole);

public record AdminServerChatMessage(
        NetworkAction NetworkAction,
        ChatDestination ChatDestination,
        uint ClientId,
        string Message,
        long Data)
    : AdminMessage(AdminMessageType.AdminPacketServerChat);

#endregion

#region AdminServerClient

public record AdminServerClientJoinMessage(
        uint ClientId)
    : AdminMessage(AdminMessageType.AdminPacketServerClientJoin);

public record AdminServerClientUpdateMessage(
        uint ClientId, 
        string ClientName, 
        byte PlayingAs)
    : AdminMessage(AdminMessageType.AdminPacketServerClientUpdate);

public record AdminServerClientInfoMessage(
        uint ClientId,
        string Hostname,
        string ClientName,
        byte Language,
        OttdDate JoinDate,
        byte PlayingAs)
    : AdminMessage(AdminMessageType.AdminPacketServerClientInfo);

public record AdminServerClientErrorMessage : AdminMessage
{
    public uint ClientId { get; }
    public NetworkErrorCode Error { get; }

    public AdminServerClientErrorMessage(uint clientId, byte error) 
        : base(AdminMessageType.AdminPacketServerClientError)
    {
        ClientId = clientId;
        Error = (NetworkErrorCode)error;
    }
}

#endregion

#region AdminServerCompany

public record AdminServerCompanyNewMessage(
        byte CompanyId)
    : AdminMessage(AdminMessageType.AdminPacketServerCompanyNew);

public record AdminServerCompanyInfoMessage(
        byte CompanyId,
        string CompanyName,
        string ManagerName,
        byte Color,
        bool HasPassword,
        OttdDate CreationDate,
        bool IsAi,
        byte MonthsOfBankruptcy,
        byte[] ShareOwnersIds)
    : AdminMessage(AdminMessageType.AdminPacketServerCompanyInfo);

public record AdminServerCompanyEconomyMessage(
        byte CompanyId,
        ulong Money,
        ulong CurrentLoan,
        ulong Income,
        ushort DeliveredCargo)
    : AdminMessage(AdminMessageType.AdminPacketServerCompanyEconomy);

public record AdminServerCompanyUpdateMessage(
        byte CompanyId,
        string CompanyName,
        string ManagerName,
        byte Color,
        bool HasPassword,
        byte MonthsOfBankruptcy,
        byte[] ShareOwnersIds)
    : AdminMessage(AdminMessageType.AdminPacketServerCompanyUpdate);

public record AdminServerCompanyRemoveMessage : AdminMessage
{
    public byte CompanyId { get; }
    public AdminCompanyRemoveReason RemoveReason { get; }

    public AdminServerCompanyRemoveMessage(byte companyId, byte removeReason)
        : base(AdminMessageType.AdminPacketServerCompanyRemove)
    {
        CompanyId = companyId;
        RemoveReason = (AdminCompanyRemoveReason)removeReason;
    }
};

#endregion
