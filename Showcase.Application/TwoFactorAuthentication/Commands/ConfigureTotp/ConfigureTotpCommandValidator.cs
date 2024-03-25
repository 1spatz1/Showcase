using FluentValidation;

namespace Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;

public class ConfigureTotpCommandValidator : AbstractValidator<ConfigureTotpCommand>
{
    public ConfigureTotpCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.")
            .Must(guid => Guid.TryParse(guid.ToString(), out _))
            .WithMessage("Invalid UserId format");
    }
}