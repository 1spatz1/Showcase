using FluentValidation;

namespace Showcase.Application.Game.Commands.ChangeGameState;

public class ChangeGameStateCommandValidator : AbstractValidator<ChangeGameStateCommand>
{
    public ChangeGameStateCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("GameId must not be empty.");
    }
}