namespace OpenTTD.Network.Models.Enums;

public enum ChatDestination
{
    /// <summary>
    /// Send message/notice to all clients (All)
    /// </summary>
    DesttypeBroadcast,
    /// <summary>
    ///  Send message/notice to everyone playing the same company (Team)
    /// </summary>
    DesttypeTeam,
    /// <summary>
    ///  Send message/not   ice to only a certain client (Private)
    /// </summary>
    DesttypeClient,
}