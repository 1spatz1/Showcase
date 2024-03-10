using Mapster;
using Showcase.Application.Authentication.Commands.Register;
using Showcase.Application.Authentication.Common;
using Showcase.Application.Authentication.Queries.Login;
using Showcase.Contracts.Authentication;

namespace Showcase.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();
        config.NewConfig<LoginRequest, LoginQuery>();
        config.NewConfig<AuthenticationResponse, AuthenticationApiResponse>();
    }
}