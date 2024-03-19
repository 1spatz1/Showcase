namespace Showcase.Contracts.Game;

public record TurnGameRequest
(
    Guid UserId,
    Guid GameId,
    int RowIndex,
    int ColIndex
);