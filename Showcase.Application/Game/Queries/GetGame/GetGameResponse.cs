namespace Showcase.Application.Game.Queries.GetGame;

public record GetGameResponse
(
    Guid UserId,
    Domain.Entities.Game Game
);