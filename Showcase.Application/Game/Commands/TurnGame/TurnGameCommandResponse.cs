namespace Showcase.Application.Game.Commands.placeTurn;

public record TurnGameCommandResponse
(
    Guid UserId, 
    Guid GameId,
    int RowIndex,
    int ColIndex
);