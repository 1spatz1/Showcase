using Mapster;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Application.Game.Commands.placeTurn;
using Showcase.Application.Game.Queries.CheckGameStatus;
using Showcase.Contracts.Game;

namespace Showcase.Api.Common.Mapping;

public class GameMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateGameRequest, CreateGameCommand>();
        config.NewConfig<TurnGameRequest, CheckGameStatusQuery>();
        config.NewConfig<CheckGameStatusResponse, TurnGameApiResponse>();
        config.NewConfig<TurnGameCommandResponse, TurnGameApiResponse>();
    }
}