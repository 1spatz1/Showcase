using ErrorOr;
using MediatR;

namespace Showcase.Application.Game.Commands.placeTurn;

public record placeTurnCommand(
    Guid UserId,
    Guid GameId,
    int RowIndex,
    int ColIndex
) : IRequest<ErrorOr<placeTurnCommandResponse>>;