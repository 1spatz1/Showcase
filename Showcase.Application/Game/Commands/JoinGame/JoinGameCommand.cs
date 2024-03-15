using ErrorOr;
using MediatR;

namespace Showcase.Application.Game.Commands.JoinGame;

public record JoinGameCommand
(
    Guid UserId,
    string Username,
    string Token,
    Guid GameId
) : IRequest<ErrorOr<JoinGameResponse>>;