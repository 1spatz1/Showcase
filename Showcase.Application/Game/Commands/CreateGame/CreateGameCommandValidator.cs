using FluentValidation;

namespace Showcase.Application.Game.Commands.CreateGame;

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.")
            .Must(guid => Guid.TryParse(guid.ToString(), out _))
            .WithMessage("Invalid UserId format");
    }
}