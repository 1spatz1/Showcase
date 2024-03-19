using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Game.Commands.ChangeGameState;

public class ChangeGameStateCommandHandler : IRequestHandler<ChangeGameStateCommand, ErrorOr<ChangeGameStateResponse>>
{
    private readonly ILogger<ChangeGameStateCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public ChangeGameStateCommandHandler(ILogger<ChangeGameStateCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ErrorOr<ChangeGameStateResponse>> Handle(ChangeGameStateCommand request,
        CancellationToken cancellationToken)
    {
        // Get the game with board
        var game = await _context.Games.Include(x => x.Board).FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
        
        // Check if Game exists
        if (game is null)
            return Errors.Game.GameNotFound;
        
        // Check if User is one of the players
        if (game.PlayerOneId != request.UserId && game.PlayerTwoId != request.UserId)
            return Errors.Authorisation.NotAllowed;
        
        // Check if the game is already over
        if (game.State is GameState.Over or GameState.Draw or GameState.Cancelled) 
            return Errors.Game.GameAlreadyOver;

        if (request.Win)
        {
            game.State = GameState.Over;
            game.WinnerId = request.UserId;
            game.FinishedAt = DateTime.UtcNow;

            if(await SaveToDb(game, cancellationToken) == true)
                return new ChangeGameStateResponse(true);
        }

        if (request.Draw)
        {
            game.State = GameState.Draw;
            game.FinishedAt = DateTime.UtcNow;
            
            if(await SaveToDb(game, cancellationToken) == true)
                return new ChangeGameStateResponse(true);
        }
        return new ChangeGameStateResponse(false);
    }
    
    private async Task<ErrorOr<Boolean>> SaveToDb(Domain.Entities.Game game, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update game with GameState in database: {Message}", ex.Message);
            return false;
        }
    }
}