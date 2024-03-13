namespace Showcase.Application.Game.Queries.CheckGameStatus;

public record CheckGameStatusResponse
(
    Guid UserId, 
    Guid GameId,
    int RowIndex,
    int ColIndex,
    bool Win,
    bool Draw
);