using Mapster;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Contracts.Game;

namespace Showcase.Api.Common.Mapping;

public class GameMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateGameRequest, CreateGameCommand>();
    }
}