using FluentValidation;

namespace Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;

public class ConfigureTotpCommandValidator : AbstractValidator<ConfigureTotpCommand>
{
    public ConfigureTotpCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username must not be empty.");
    }
}