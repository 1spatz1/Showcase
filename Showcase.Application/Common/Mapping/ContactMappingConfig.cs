using Mapster;
using Showcase.Application.Contact.Commands;
using Showcase.Infrastructure.Email.Commands;
using Showcase.Infrastructure.Recaptcha.Queries;

namespace Showcase.Application.Common.Mapping;

public class ContactMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ContactCommand, ValidateRecaptchaQuery>();
        config.NewConfig<ContactCommand, SendEmailCommand>();
    }
}