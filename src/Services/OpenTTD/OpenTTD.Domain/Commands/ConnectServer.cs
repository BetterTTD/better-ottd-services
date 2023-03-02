﻿using MediatR;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Commands;

public sealed record ConnectServer(ServerId Id) : INotification;