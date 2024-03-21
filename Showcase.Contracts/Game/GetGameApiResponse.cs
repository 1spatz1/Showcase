namespace Showcase.Contracts.Game;

public record GetGameApiResponse(Guid UserId, Domain.Entities.Game Game)
{
    
}