namespace Showcase.Application.Authentication.Commands.ConfigureTotp;

public record ConfigureTotpResponse
    (
        string QrCodeUri,
        string SharedKey
    );