namespace Showcase.Contracts.Game;

public record CreateGameApiResponse(Guid UserId, string Username, string Token, Guid GameId)
{
}