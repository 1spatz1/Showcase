using FluentValidation;

namespace Showcase.Application.Game.Commands.placeTurn;

public class placeTurnCommandValidator : AbstractValidator<placeTurnCommand>
{
    public placeTurnCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("GameId must not be empty.");
        RuleFor(x => x.RowIndex)
            .NotEmpty()
            .WithMessage("RowIndex must not be empty.");
        RuleFor(x => x.ColIndex)
            .NotEmpty()
            .WithMessage("ColIndex must not be empty.");
    }
}