using ErrorOr;
using MediatR;

namespace Showcase.Application.Game.Commands.ChangeGameState;

public record ChangeGameStateCommand
(
    Guid UserId, 
    Guid GameId,
    bool? Win,
    bool? Draw
) : IRequest<ErrorOr<ChangeGameStateResponse>>;