namespace Showcase.Application.Game.Commands.placeTurn;

public record placeTurnCommandResponse
(
    Guid UserId, 
    Guid GameId,
    int RowIndex,
    int ColIndex
);