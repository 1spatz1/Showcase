using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;

namespace Showcase.Application.Game.Commands.CreateGame;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, ErrorOr<CreateGameResponse>>
{
    private readonly ILogger<CreateGameCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public CreateGameCommandHandler(ILogger<CreateGameCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ErrorOr<CreateGameResponse>> Handle(CreateGameCommand request,
        CancellationToken cancellationToken)
    {
        var newGame = new Domain.Entities.Game
        {
            Id = Guid.NewGuid(),
            PlayerOneId = request.UserId,
            BoardSize = 3,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        
        try
        {
            await _context.Games.AddAsync(newGame, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save Game to database: {Message}", ex.Message);
            return Errors.UnexpectedError;
        }


        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save Game to database: {Message}", ex.Message);
            return Errors.UnexpectedError;
        }
        
        return new CreateGameResponse(request.UserId, request.Username, request.Token, newGame.Id);
    }
}