﻿using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Outbound.Ping;

public sealed record PingMessage(uint Argument = 0) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_PING;
}