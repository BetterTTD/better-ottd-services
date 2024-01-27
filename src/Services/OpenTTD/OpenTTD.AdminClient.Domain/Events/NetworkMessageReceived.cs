﻿using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.AdminClient.Networking.Messages;

namespace OpenTTD.AdminClient.Domain.Events;

public sealed record NetworkMessageReceived(ServerId ServerId, IMessage Message) : BaseEvent;