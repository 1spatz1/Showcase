﻿using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Showcase.Api.Routes;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Application.Game.Commands.placeTurn;
using Showcase.Contracts.Game;
using Showcase.Domain.Identity;

namespace Showcase.Api.Controllers;

[Route(V1Routes.Game.Controller)]
[Authorize]
public class GameController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GameController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost(V1Routes.Game.Create)]
    public async Task<IActionResult> Create([FromBody] CreateGameRequest request)
    {
        CreateGameCommand command = _mapper.Map<CreateGameCommand>(request);
        ErrorOr<CreateGameCommandResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<CreateGameApiResponse>(value)), Problem);
    }
    
    [HttpPost(V1Routes.Game.Turn)]
    public async Task<IActionResult> Turn([FromBody] TurnGameRequest request)
    {
        TurnGameCommand command = _mapper.Map<TurnGameCommand>(request);
        ErrorOr<TurnGameCommandResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<TurnGameApiResponse>(value)), Problem);
    }
    
    [HttpGet("test")]
    [Authorize (Roles = IdentityNames.Roles.Administrator)]
    public async Task<IActionResult> Test()
    {
        return StatusCode(501);
    }
}