using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Showcase.Api.Routes;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Application.Contact.Commands;
using Showcase.Application.Contact.Common;
using Showcase.Contracts.Contact;

namespace Showcase.Api.Controllers
{
    [Route(V1Routes.Contact.Controller)]
    [ApiController]
    public class ContactController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ContactController(IMapper mapper, IMediator mediator, IJwtTokenService tokenService)
            : base(tokenService)
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        
        [HttpPost(V1Routes.Contact.SendEmail)]
        public async Task<IActionResult> SendEmail([FromBody] ContactRequest request)
        {
            ContactCommand command = _mapper.Map<ContactCommand>(request);
            ErrorOr<ContactResponse> response = await _mediator.Send(command);

            return response.Match(value => Ok(_mapper.Map<ContactApiResponse>(value)), Problem);
        }
    }
}
