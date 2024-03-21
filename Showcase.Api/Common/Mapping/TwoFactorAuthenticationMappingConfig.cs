using Mapster;
using Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.DisableTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;
using Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;
using Showcase.Contracts.TwoFactorAuthentication;

namespace Showcase.Api.Common.Mapping;

public class TwoFactorAuthenticationaMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ConfigureTotpRequest, ConfigureTotpCommand>();
        config.NewConfig<ConfigureTotpResponse, ConfigureTotpApiResponse>();
        config.NewConfig<DisableTotpRequest, DisableTotpCommand>();
        config.NewConfig<DisableTotpResponse, DisableTotpApiResponse>();
        config.NewConfig<EnableTotpRequest, EnableTotpCommand>();
        config.NewConfig<EnableTotpResponse, EnableTotpApiResponse>();
        config.NewConfig<VerifyTotpRequest, VerifyTotpQuery>();
        config.NewConfig<VerifyTotpResponse, VerifyTotpApiResponse>();
        config.NewConfig<EnableTotpCommand, VerifyTotpQuery>();
    }
}