namespace OpenTTD.API.Network.AdminPort;

public enum AdminUpdateType
{
	AdminUpdateDate,            /// Updates about the date of the game.
	AdminUpdateClientInfo,     /// Updates about the information of clients.
	AdminUpdateCompanyInfo,    /// Updates about the generic information of companies.
	AdminUpdateCompanyEconomy, /// Updates about the economy of companies.
	AdminUpdateCompanyStats,   /// Updates about the statistics of companies.
	AdminUpdateChat,            /// The admin would like to have chat messages.
	AdminUpdateConsole,         /// The admin would like to have console messages.
	AdminUpdateCmdNames,       /// The admin would like a list of all DoCommand names.
	AdminUpdateCmdLogging,     /// The admin would like to have DoCommand information.
	AdminUpdateGamescript,      /// The admin would like to have gamescript messages.
	AdminUpdateEnd,             /// Must ALWAYS be on the end of this list!! (period)
}