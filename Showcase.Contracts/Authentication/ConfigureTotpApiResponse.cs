namespace Showcase.Contracts.Authentication;

public record ConfigureTotpApiResponse(string QrCodeUri, string SharedKey)
{
    
}