namespace Showcase.Contracts.Game;

public record GetGameRequest
(
    Guid GameId,
    string UserId = ""
);