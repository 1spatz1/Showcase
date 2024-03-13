namespace Showcase.Contracts.Game;

public record TurnGameApiResponse(Guid UserId, Guid GameId, int RowIndex, int ColIndex)
{
}