using Mapster;
using Showcase.Application.Authentication.Commands.LockUser;
using Showcase.Application.Authentication.Commands.UnlockUser;
using Showcase.Contracts.Moderation;

namespace Showcase.Api.Common.Mapping;

public class AdminMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LockUserRequest, LockUserCommand>();
        config.NewConfig<UnlockUserRequest, UnlockUserCommand>();
        config.NewConfig<LockUserResponse, LockUserApiResponse>();
        config.NewConfig<UnlockUserResponse, UnlockUserApiResponse>();
    }
}
