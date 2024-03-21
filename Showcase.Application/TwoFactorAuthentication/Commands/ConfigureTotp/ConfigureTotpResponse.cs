namespace Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;

public record ConfigureTotpResponse
    (
        string QrCodeUri,
        string SharedKey
    );