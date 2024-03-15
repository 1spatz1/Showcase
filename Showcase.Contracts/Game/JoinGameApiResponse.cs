namespace Showcase.Contracts.Game;

public record JoinGameApiResponse(Guid UserId, string userName, string Token, Domain.Entities.Game Game)
{
    
}