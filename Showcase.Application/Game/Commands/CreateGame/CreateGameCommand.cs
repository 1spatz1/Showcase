﻿using ErrorOr;
using MediatR;

namespace Showcase.Application.Game.Commands.CreateGame;

public record CreateGameCommand
(
    Guid UserId,
    string Username,
    string Token
) : IRequest<ErrorOr<CreateGameResponse>>;