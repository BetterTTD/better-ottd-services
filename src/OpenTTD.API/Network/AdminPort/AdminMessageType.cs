namespace OpenTTD.API.Network.AdminPort;

public enum AdminMessageType
{
	AdminPacketAdminJoin,             /// The admin announces and authenticates itself to the server.
	AdminPacketAdminQuit,             /// The admin tells the server that it is quitting.
	AdminPacketAdminUpdateFrequency, /// The admin tells the server the update frequency of a particular piece of information.
	AdminPacketAdminPoll,             /// The admin explicitly polls for a piece of information.
	AdminPacketAdminChat,             /// The admin sends a chat message to be distributed.
	AdminPacketAdminRcon,             /// The admin sends a remote console command.
	AdminPacketAdminGamescript,       /// The admin sends a JSON string for the GameScript.
	AdminPacketAdminPing,             /// The admin sends a ping to the server, expecting a ping-reply (PONG) packet.

	AdminPacketServerFull = 100,      /// The server tells the admin it cannot accept the admin.
	AdminPacketServerBanned,          /// The server tells the admin it is banned.
	AdminPacketServerError,           /// The server tells the admin an error has occurred.
	AdminPacketServerProtocol,        /// The server tells the admin its protocol version.
	AdminPacketServerWelcome,         /// The server welcomes the admin to a game.
	AdminPacketServerNewgame,         /// The server tells the admin its going to start a new game.
	AdminPacketServerShutdown,        /// The server tells the admin its shutting down.

	AdminPacketServerDate,            /// The server tells the admin what the current game date is.
	AdminPacketServerClientJoin,     /// The server tells the admin that a client has joined.
	AdminPacketServerClientInfo,     /// The server gives the admin information about a client.
	AdminPacketServerClientUpdate,   /// The server gives the admin an information update on a client.
	AdminPacketServerClientQuit,     /// The server tells the admin that a client quit.
	AdminPacketServerClientError,    /// The server tells the admin that a client caused an error.
	AdminPacketServerCompanyNew,     /// The server tells the admin that a new company has started.
	AdminPacketServerCompanyInfo,    /// The server gives the admin information about a company.
	AdminPacketServerCompanyUpdate,  /// The server gives the admin an information update on a company.
	AdminPacketServerCompanyRemove,  /// The server tells the admin that a company was removed.
	AdminPacketServerCompanyEconomy, /// The server gives the admin some economy related company information.
	AdminPacketServerCompanyStats,   /// The server gives the admin some statistics about a company.
	AdminPacketServerChat,            /// The server received a chat message and relays it.
	AdminPacketServerRcon,            /// The server's reply to a remove console command.
	AdminPacketServerConsole,         /// The server gives the admin the data that got printed to its console.
	AdminPacketServerCmdNames,       /// The server sends out the names of the DoCommands to the admins.
	AdminPacketServerCmdLogging,     /// The server gives the admin copies of incoming command packets.
	AdminPacketServerGamescript,      /// The server gives the admin information from the GameScript in JSON.
	AdminPacketServerRconEnd,        /// The server indicates that the remote console command has completed.
	AdminPacketServerPong,            /// The server replies to a ping request from the admin.

	InvalidAdminPacket = 0xFF,         /// An invalid marker for admin packets.
}