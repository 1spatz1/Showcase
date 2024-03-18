namespace Showcase.Contracts.Game;

public record GetGameRequest
(
    Guid UserId,
    string Username,
    string Token,
    Guid GameId    
);