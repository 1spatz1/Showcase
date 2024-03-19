namespace Showcase.Application.Game.Commands.placeTurn;

public record TurnGameResponse
(
    Guid UserId, 
    Guid GameId,
    int RowIndex,
    int ColIndex
);