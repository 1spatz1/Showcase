namespace Showcase.Contracts.TwoFactorAuthentication;

public record ConfigureTotpApiResponse(string QrCodeUri, string SharedKey)
{
    
}