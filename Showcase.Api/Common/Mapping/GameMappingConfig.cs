using Mapster;
using Showcase.Application.Game.Commands.ChangeGameState;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Application.Game.Commands.JoinGame;
using Showcase.Application.Game.Commands.placeTurn;
using Showcase.Application.Game.Queries.CheckGameStatus;
using Showcase.Application.Game.Queries.GetGame;
using Showcase.Contracts.Game;

namespace Showcase.Api.Common.Mapping;

public class GameMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateGameRequest, CreateGameCommand>();
        config.NewConfig<TurnGameRequest, CheckGameStatusQuery>();
        config.NewConfig<CheckGameStatusResponse, TurnGameApiResponse>();
        config.NewConfig<TurnGameResponse, TurnGameApiResponse>();
        config.NewConfig<JoinGameRequest, JoinGameCommand>();
        config.NewConfig<JoinGameResponse, JoinGameApiResponse>();
        config.NewConfig<CheckGameStatusResponse, ChangeGameStateCommand>();
        config.NewConfig<GetGameRequest, GetGameQuery>();
        config.NewConfig<GetGameResponse, GetGameApiResponse>();
    }
}