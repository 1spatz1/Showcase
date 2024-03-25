using FluentValidation;

namespace Showcase.Application.Game.Commands.JoinGame;

public class JoinGameCommandValidator : AbstractValidator<JoinGameCommand>
{
    public JoinGameCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.")
            .Must(guid => Guid.TryParse(guid.ToString(), out _))
            .WithMessage("Invalid UserId format");
        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("GameId must not be empty.")
            .Must(guid => Guid.TryParse(guid.ToString(), out _))
            .WithMessage("Invalid GameId format");
    }
}