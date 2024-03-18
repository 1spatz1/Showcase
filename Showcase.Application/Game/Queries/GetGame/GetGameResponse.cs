namespace Showcase.Application.Game.Queries.GetGame;

public record GetGameResponse
(
    Guid UserId,
    string Username,
    string Token,
    Domain.Entities.Game Game
);