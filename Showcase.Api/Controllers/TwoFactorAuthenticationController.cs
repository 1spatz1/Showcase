using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Showcase.Api.Routes;
using Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.DisableTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;
using Showcase.Contracts.TwoFactorAuthentication;

namespace Showcase.Api.Controllers;

[Authorize]
[Route(V1Routes.TwoFactorAuthentication.Controller)]
public class TwoFactorAuthenticationController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TwoFactorAuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost(V1Routes.TwoFactorAuthentication.Configure)]
    public async Task<IActionResult> Configure([FromBody] ConfigureTotpRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        ConfigureTotpCommand command = _mapper.Map<ConfigureTotpCommand>(request);
        ErrorOr<ConfigureTotpResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<ConfigureTotpApiResponse>(value)), Problem);
    }
    
    [HttpPost(V1Routes.TwoFactorAuthentication.Disable)]
    public async Task<IActionResult> Disable([FromBody] DisableTotpRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        DisableTotpCommand command = _mapper.Map<DisableTotpCommand>(request);
        ErrorOr<DisableTotpResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<DisableTotpApiResponse>(value)), Problem);
    }
    
    [HttpPost(V1Routes.TwoFactorAuthentication.Enable)]
    public async Task<IActionResult> Enable([FromBody] EnableTotpRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        EnableTotpCommand command = _mapper.Map<EnableTotpCommand>(request);
        ErrorOr<EnableTotpResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<EnableTotpApiResponse>(value)), Problem);
    }
}