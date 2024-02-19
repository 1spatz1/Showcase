using ErrorOr;
using MapsterMapper;
using MediatR;

using Showcase.Application.Contact.Common;
using Showcase.Domain.Common.Errors;
using Showcase.Infrastructure.Email.Commands;
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
        ValidateRecaptchaQuery validateRecaptchaQuery = _mapper.Map<ValidateRecaptchaQuery>(request);
        ErrorOr<ValidateRecaptchaResponse> recaptchaResponse = await _mediator.Send(validateRecaptchaQuery, cancellationToken);
        
        if (recaptchaResponse.IsError || recaptchaResponse.Value.Succes == false)
            return Errors.Authorisation.ReCaptchaFailed;

        SendEmailCommand sendEmailCommand = _mapper.Map<SendEmailCommand>(request);
        ErrorOr<SendEmailResponse> sendEmailResponse = await _mediator.Send(sendEmailCommand, cancellationToken);
        
        if (sendEmailResponse.IsError)
            return Errors.UnexpectedError;
        
        return new ContactResponse("Email sent");
    }
}