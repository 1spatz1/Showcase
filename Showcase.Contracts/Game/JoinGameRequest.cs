namespace Showcase.Contracts.Game;

public record JoinGameRequest
(
    Guid GameId,
    string RecaptchaToken,
    string UserId = ""
);