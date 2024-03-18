namespace Showcase.Contracts.Game;

public record GetGameApiResponse(Guid UserId, string userName, string Token, Domain.Entities.Game Game)
{
    
}