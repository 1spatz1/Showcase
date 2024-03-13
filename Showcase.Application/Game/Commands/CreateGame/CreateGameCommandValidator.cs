using FluentValidation;

namespace Showcase.Application.Game.Commands.CreateGame;

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username must not be empty.");
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token must not be empty.");
    }
}