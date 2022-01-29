module OpenTTD.AdminClient.Networking.Enums

type PacketType =
    | ADMIN_PACKET_ADMIN_JOIN              = 0 
    | ADMIN_PACKET_ADMIN_QUIT              = 1 
    | ADMIN_PACKET_ADMIN_UPDATE_FREQUENCY  = 2 
    | ADMIN_PACKET_ADMIN_POLL              = 3 
    | ADMIN_PACKET_ADMIN_CHAT              = 4 
    | ADMIN_PACKET_ADMIN_RCON              = 5
    | ADMIN_PACKET_ADMIN_GAMESCRIPT        = 6 
    | ADMIN_PACKET_ADMIN_PING              = 7
    
    | ADMIN_PACKET_SERVER_FULL             = 100
    | ADMIN_PACKET_SERVER_BANNED           = 101
    | ADMIN_PACKET_SERVER_ERROR            = 102
    | ADMIN_PACKET_SERVER_PROTOCOL         = 103
    | ADMIN_PACKET_SERVER_WELCOME          = 104
    | ADMIN_PACKET_SERVER_NEWGAME          = 105
    | ADMIN_PACKET_SERVER_SHUTDOWN         = 106
    | ADMIN_PACKET_SERVER_DATE             = 107
    | ADMIN_PACKET_SERVER_CLIENT_JOIN      = 108
    | ADMIN_PACKET_SERVER_CLIENT_INFO      = 109
    | ADMIN_PACKET_SERVER_CLIENT_UPDATE    = 110
    | ADMIN_PACKET_SERVER_CLIENT_QUIT      = 111
    | ADMIN_PACKET_SERVER_CLIENT_ERROR     = 112
    | ADMIN_PACKET_SERVER_COMPANY_NEW      = 113
    | ADMIN_PACKET_SERVER_COMPANY_INFO     = 114
    | ADMIN_PACKET_SERVER_COMPANY_UPDATE   = 115
    | ADMIN_PACKET_SERVER_COMPANY_REMOVE   = 116
    | ADMIN_PACKET_SERVER_COMPANY_ECONOMY  = 117
    | ADMIN_PACKET_SERVER_COMPANY_STATS    = 118
    | ADMIN_PACKET_SERVER_CHAT             = 119
    | ADMIN_PACKET_SERVER_RCON             = 120
    | ADMIN_PACKET_SERVER_CONSOLE          = 121
    | ADMIN_PACKET_SERVER_CMD_NAMES        = 122
    | ADMIN_PACKET_SERVER_CMD_LOGGING      = 123
    | ADMIN_PACKET_SERVER_GAMESCRIPT       = 124
    | ADMIN_PACKET_SERVER_RCON_END         = 125
    | ADMIN_PACKET_SERVER_PONG             = 126
    | ADMIN_PACKET_SERVER_END              = 127
    
    | INVALID_ADMIN_PACKET                 = 0xFF

type AdminCompanyRemoveReason =
    | ADMIN_CRR_MANUAL                     = 0
    | ADMIN_CRR_AUTOCLEAN                  = 1
    | ADMIN_CRR_BANKRUPT                   = 2
    
type AdminUpdateFrequency =
    | ADMIN_FREQUENCY_POLL                 = 0x01
    | ADMIN_FREQUENCY_DAILY                = 0x02
    | ADMIN_FREQUENCY_WEEKLY               = 0x04
    | ADMIN_FREQUENCY_MONTHLY              = 0x08
    | ADMIN_FREQUENCY_QUARTERLY            = 0x10
    | ADMIN_FREQUENCY_ANUALLY              = 0x20
    | ADMIN_FREQUENCY_AUTOMATIC            = 0x40
    
type AdminUpdateType =
    | ADMIN_UPDATE_DATE                    = 0
    | ADMIN_UPDATE_CLIENT_INFO             = 1
    | ADMIN_UPDATE_COMPANY_INFO            = 2
    | ADMIN_UPDATE_COMPANY_ECONOMY         = 3
    | ADMIN_UPDATE_COMPANY_STATS           = 4
    | ADMIN_UPDATE_CHAT                    = 5
    | ADMIN_UPDATE_CONSOLE                 = 6
    | ADMIN_UPDATE_CMD_NAMES               = 7
    | ADMIN_UPDATE_CMD_LOGGING             = 8
    | ADMIN_UPDATE_GAMESCRIPT              = 9
    | ADMIN_UPDATE_END                     = 10
    
type DestType =
    | DESTTYPE_BROADCAST                   = 0
    | DESTTYPE_TEAM                        = 1
    | DESTTYPE_CLIENT                      = 2
    
type NetworkAction =
    | NETWORK_ACTION_JOIN                  = 0
    | NETWORK_ACTION_LEAVE                 = 1
    | NETWORK_ACTION_SERVER_MESSAGE        = 2
    | NETWORK_ACTION_CHAT                  = 3
    | NETWORK_ACTION_CHAT_COMPANY          = 4
    | NETWORK_ACTION_CHAT_CLIENT           = 5
    | NETWORK_ACTION_GIVE_MONEY            = 6
    | NETWORK_ACTION_NAME_CHANGE           = 7
    | NETWORK_ACTION_COMPANY_SPECTATOR     = 8
    | NETWORK_ACTION_COMPANY_JOIN          = 9
    | NETWORK_ACTION_COMPANY_NEW           = 10
    
type NetworkErrorCode =
    | NETWORK_ERROR_GENERAL                = 0
    | NETWORK_ERROR_DESYNC                 = 1
    | NETWORK_ERROR_SAVEGAME_FAILED        = 2
    | NETWORK_ERROR_CONNECTION_LOST        = 3
    | NETWORK_ERROR_ILLEGAL_PACKET         = 4
    | NETWORK_ERROR_NEWGRF_MISMATCH        = 5
    | NETWORK_ERROR_NOT_AUTHORIZED         = 6
    | NETWORK_ERROR_NOT_EXPECTED           = 7
    | NETWORK_ERROR_WRONG_REVISION         = 8
    | NETWORK_ERROR_NAME_IN_USE            = 9
    | NETWORK_ERROR_WRONG_PASSWORD         = 10
    | NETWORK_ERROR_COMPANY_MISMATCH       = 11
    | NETWORK_ERROR_KICKED                 = 12
    | NETWORK_ERROR_CHEATER                = 13
    | NETWORK_ERROR_FULL                   = 14
    
type NetworkLanguage =
    | NETLANG_ANY                          = 0
    
type PauseMode =
    | PM_UNPAUSED                          = 0
    | PM_PAUSED_NORMAL                     = 1
    | PM_PAUSED_SAVELOAD                   = 2
    | PM_PAUSED_JOIN                       = 4
    | PM_PAUSED_ERROR                      = 8
    | PM_PAUSED_ACTIVE_CLIENTS             = 16
    | PM_PAUSED_GAME_SCRIPT                = 32
    
type VehicleType =
    | NETWORK_VEH_TRAIN                    = 0
    | NETWORK_VEH_LORRY                    = 1
    | NETWORK_VEH_BUS                      = 2
    | NETWORK_VEH_PLANE                    = 3
    | NETWORK_VEH_SHIP                     = 4
    
type ChatDestination =
    | DESTTYPE_BROADCAST                   = 0
    | DESTTYPE_TEAM                        = 1
    | DESTTYPE_CLIENT                      = 2