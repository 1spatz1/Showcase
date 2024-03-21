using FluentValidation;

namespace Showcase.Application.Game.Queries.GetGame;

public class GetGameQueryValidator : AbstractValidator<GetGameQuery>
{
    public GetGameQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("GameId must not be empty.");
    }
}