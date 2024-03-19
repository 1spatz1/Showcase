using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Showcase.Api.Routes;
using Showcase.Application.Authentication.Commands.Register;
using Showcase.Application.Authentication.Common;
using Showcase.Application.Authentication.Queries.Login;
using Showcase.Contracts.Authentication;
using LoginRequest = Showcase.Contracts.Authentication.LoginRequest;
using RegisterRequest = Showcase.Contracts.Authentication.RegisterRequest;

namespace Showcase.Api.Controllers;

[Route(V1Routes.Authentication.Controller)]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost(V1Routes.Authentication.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        LoginQuery query = _mapper.Map<LoginQuery>(request);
        ErrorOr<AuthenticationResponse> response = await _mediator.Send(query);
        
        return response.Match(value => Ok(_mapper.Map<AuthenticationApiResponse>(value)), Problem);
    }
    
    [HttpPost(V1Routes.Authentication.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        RegisterCommand command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<AuthenticationApiResponse>(value)), Problem);
    }
}