using Mapster;
using Showcase.Application.Contact.Commands;
using Showcase.Application.Contact.Common;
using Showcase.Contracts.Contact;

namespace Showcase.Api.Common.Mapping;

public class ContactMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ContactRequest, ContactCommand>();
        config.NewConfig<ContactResponse, ContactApiResponse>();
    }
}