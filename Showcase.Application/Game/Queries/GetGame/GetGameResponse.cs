namespace Showcase.Application.Game.Queries.GetGame;

public record GetGameResponse
(
    Guid UserId,
    string Username,
    Domain.Entities.Game Game
);