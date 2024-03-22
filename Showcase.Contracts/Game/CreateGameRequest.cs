namespace Showcase.Contracts.Game;

public record CreateGameRequest
(
    string RecaptchaToken,
    string UserId = ""
);