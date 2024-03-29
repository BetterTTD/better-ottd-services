﻿using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerProtocol;

public sealed record ServerProtocolMessage(
    byte NetworkVersion, 
    Dictionary<UpdateType, Enums.UpdateFrequency> AdminUpdateSettings) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PROTOCOL;
}