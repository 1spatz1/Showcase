namespace Showcase.Contracts.Game;

public record GetGameApiResponse(Guid UserId, string UserName, string Token, Domain.Entities.Game Game)
{
    
}