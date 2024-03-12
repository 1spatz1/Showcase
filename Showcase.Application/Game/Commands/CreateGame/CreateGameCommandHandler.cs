using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Showcase.Application.Authentication.Commands.Register;

namespace Showcase.Application.Game.Commands.CreateGame;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, ErrorOr<CreateGameResponse>>
{
    private readonly ILogger<CreateGameCommandHandler> _logger;

    public CreateGameCommandHandler(ILogger<CreateGameCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<ErrorOr<CreateGameResponse>> Handle(CreateGameCommand request,
        CancellationToken cancellationToken)
    {
        
        return new CreateGameResponse(request.UserId, request.Username, request.Token, Guid.NewGuid());
    }
}