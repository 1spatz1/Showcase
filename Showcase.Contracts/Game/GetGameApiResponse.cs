namespace Showcase.Contracts.Game;

public record GetGameApiResponse(Guid UserId, string Username, Domain.Entities.Game Game)
{
    
}