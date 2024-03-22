namespace Showcase.Contracts.Game;

public record TurnGameRequest
(
    Guid GameId,
    int RowIndex,
    int ColIndex,
    string RecaptchaToken,
    string UserId = ""
);