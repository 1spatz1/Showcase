using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Showcase.Api.Routes;
using Showcase.Application.Admin.Queries.GetUnlockedUsers;
using Showcase.Application.Authentication.Commands.LockUser;
using Showcase.Application.Authentication.Commands.UnlockUser;
using Showcase.Contracts.Admin;
using Showcase.Domain.Identity;

namespace Showcase.Api.Controllers;

[Route(V1Routes.Admin.Controller)]
[Authorize (Roles = IdentityNames.Roles.Administrator)]
public class AdminController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public AdminController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost(V1Routes.Admin.LockUser)]
    public async Task<IActionResult> LockUser([FromBody] LockUserRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        LockUserCommand command = _mapper.Map<LockUserCommand>(request);
        ErrorOr<LockUserResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<LockUserApiResponse>(value)), Problem);
    }
    
    [HttpPost(V1Routes.Admin.UnlockUser)]
    public async Task<IActionResult> UnlockUser([FromBody] UnlockUserRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }
        
        UnlockUserCommand command = _mapper.Map<UnlockUserCommand>(request);
        ErrorOr<UnlockUserResponse> response = await _mediator.Send(command);
        
        return response.Match(value => Ok(_mapper.Map<UnlockUserApiResponse>(value)), Problem);
    }
    
    [HttpGet(V1Routes.Admin.GetAllUsers)]
    public async Task<IActionResult> GetLockedUsers()
    {
        GetAllUsersQuery query = _mapper.Map<GetAllUsersQuery>(new GetAllUsersRequest());
        ErrorOr<GetAllUsersResponse> response = await _mediator.Send(query);

        return response.Match(value => Ok(_mapper.Map<GetAllUsersApiResponse>(value)), Problem);
    }
    
}