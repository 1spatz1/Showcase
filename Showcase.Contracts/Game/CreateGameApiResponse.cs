namespace Showcase.Contracts.Game;

public record CreateGameApiResponse(Guid UserId, string Token, Guid GameId)
{
}