namespace Showcase.Contracts.Game;

public record GetGameRequest
(
    Guid UserId,
    string Username,
    Guid GameId    
);