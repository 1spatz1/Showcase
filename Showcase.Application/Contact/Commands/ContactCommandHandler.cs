using ErrorOr;
using MapsterMapper;
using MediatR;

using Showcase.Application.Contact.Common;
using Showcase.Domain.Common.Errors;
using Showcase.Infrastructure.Recaptcha.Queries;

namespace Showcase.Application.Contact.Commands;

public class ContactCommandHandler : IRequestHandler<ContactCommand, ErrorOr<ContactResponse>>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ContactCommandHandler(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    public async Task<ErrorOr<ContactResponse>> Handle(ContactCommand request, CancellationToken cancellationToken)
    {
        RecaptchaQuery query = _mapper.Map<RecaptchaQuery>(request);
        ErrorOr<RecaptchaResponse> response = await _mediator.Send(query, cancellationToken);
        
        if (response.IsError || response.Value.Succes == false)
            return Errors.Authorisation.ReCaptchaFailed;
        
        return new ContactResponse("Email sent");
    }
}