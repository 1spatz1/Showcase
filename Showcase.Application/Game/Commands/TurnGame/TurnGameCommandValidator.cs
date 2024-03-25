using FluentValidation;

namespace Showcase.Application.Game.Commands.placeTurn;

public class TurnGameCommandValidator : AbstractValidator<TurnGameCommand>
{
    public TurnGameCommandValidator()
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
        RuleFor(x => x.RowIndex)
            .GreaterThan(-1)
            .WithMessage("RowIndex must not be empty.");
        RuleFor(x => x.ColIndex)
            .GreaterThan(-1)
            .WithMessage("ColIndex must not be empty.");
    }
}