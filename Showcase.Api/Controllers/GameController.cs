using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Showcase.Api.Routes;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Application.Game.Commands.ChangeGameState;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Application.Game.Commands.JoinGame;
using Showcase.Application.Game.Commands.placeTurn;
using Showcase.Application.Game.Queries.CheckGameStatus;
using Showcase.Application.Game.Queries.GetGame;
using Showcase.Contracts.Game;
using Showcase.Domain.Identity;

namespace Showcase.Api.Controllers;

[Route(V1Routes.Game.Controller)]
[Authorize]
public class GameController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GameController(IMediator mediator, IMapper mapper, IJwtTokenService tokenService)
        : base(tokenService)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost(V1Routes.Game.Create)]
    public async Task<IActionResult> Create([FromBody] CreateGameRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        CreateGameRequest requestWithUserId = request with 
        {
            UserId = await GetUserIdFromTokenAsync()
        };
        
        CreateGameCommand command = _mapper.Map<CreateGameCommand>(requestWithUserId);
        ErrorOr<CreateGameResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<CreateGameApiResponse>(value)), Problem);
    }

    [HttpPost(V1Routes.Game.Join)]
    public async Task<IActionResult> Join([FromBody] JoinGameRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        JoinGameRequest requestWithUserId = request with 
        {
            UserId = await GetUserIdFromTokenAsync()
        };
        
        JoinGameCommand command = _mapper.Map<JoinGameCommand>(requestWithUserId);
        ErrorOr<JoinGameResponse> response = await _mediator.Send(command);

        return response.Match(value => Ok(_mapper.Map<JoinGameApiResponse>(value)), Problem);
    }
    
    [HttpPost(V1Routes.Game.Turn)]
    public async Task<IActionResult> Turn([FromBody] TurnGameRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        TurnGameRequest requestWithUserId = request with 
        {
            UserId = await GetUserIdFromTokenAsync()
        };
        
        TurnGameCommand turnCommand = _mapper.Map<TurnGameCommand>(requestWithUserId);
        ErrorOr<TurnGameResponse> turnResponse = await _mediator.Send(turnCommand);
        
        if (turnResponse.IsError)
            return Problem(turnResponse.Errors);
        
        CheckGameStatusQuery checkGameStatusQuery = _mapper.Map<CheckGameStatusQuery>(requestWithUserId);
        ErrorOr<CheckGameStatusResponse> checkGameStatusResponse = await _mediator.Send(checkGameStatusQuery);

        if (checkGameStatusResponse.Value.Draw || checkGameStatusResponse.Value.Win)
        {
            ChangeGameStateCommand changeGameStateCommand = _mapper.Map<ChangeGameStateCommand>(checkGameStatusResponse.Value);
            ErrorOr<ChangeGameStateResponse> changeGameStateResponse = await _mediator.Send(changeGameStateCommand);
        }
        
        return checkGameStatusResponse.Match(value => Ok(_mapper.Map<TurnGameApiResponse>(value)), Problem);
    }
    
    [HttpPost(V1Routes.Game.Get)]
    public async Task<IActionResult> Get([FromBody] GetGameRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        GetGameRequest requestWithUserId = request with 
        {
            UserId = await GetUserIdFromTokenAsync()
        };
        
        GetGameQuery command = _mapper.Map<GetGameQuery>(requestWithUserId);
        ErrorOr<GetGameResponse> response = await _mediator.Send(command);

        return response.Match(value => Ok(_mapper.Map<GetGameApiResponse>(value)), Problem);
    }
}