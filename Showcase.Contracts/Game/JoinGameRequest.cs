namespace Showcase.Contracts.Game;

public record JoinGameRequest
(
    Guid UserId,
    string Username,
    string Token,
    Guid GameId    
);