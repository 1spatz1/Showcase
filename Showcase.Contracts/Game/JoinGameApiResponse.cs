namespace Showcase.Contracts.Game;

public record JoinGameApiResponse(Guid UserId, string Username,  Guid GameId)
{
    
}
