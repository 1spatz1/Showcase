using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Showcase.Api.Routes;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Contracts.Game;

namespace Showcase.Api.Controllers;

[Route(V1Routes.Game.Controller)]
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
        ErrorOr<CreateGameResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<CreateGameApiResponse>(value)), Problem);
    }
}