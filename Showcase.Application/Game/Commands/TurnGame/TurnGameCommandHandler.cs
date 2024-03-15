using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Application.Game.Commands.placeTurn;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Game.Commands.TurnGame;

public class TurnGameCommandHandler : IRequestHandler<TurnGameCommand, ErrorOr<TurnGameResponse>>
{
    private readonly ILogger<CreateGameCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public TurnGameCommandHandler(ILogger<CreateGameCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<ErrorOr<TurnGameResponse>> Handle(TurnGameCommand request, CancellationToken cancellationToken)
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
        
        // Check if Player is not the current turn
        if (game.PlayerTurn != request.UserId)
            return Errors.Game.GameNotYourTurn;
        
        // Check if move is valid
        if(request.ColIndex < 0 || request.ColIndex >= game.BoardSize || request.RowIndex < 0 || request.RowIndex >= game.BoardSize)
            return Errors.Game.InvalidMove;
        
        // Check if the position is already taken
        if (game.Board!.Any(bp => bp.RowIndex == request.RowIndex && bp.ColIndex == request.ColIndex))
            return Errors.Game.InvalidMove;
        
        BoardPosition boardPosition = new()
        {
            Id = Guid.NewGuid(),
            PlayerGuid = request.UserId,
            GameId = request.GameId,
            RowIndex = request.RowIndex,
            ColIndex = request.ColIndex,
            CreatedAt = DateTime.UtcNow
        };
        
        // Switch the player turn
        SwitchPlayerTurn(game); 
        game.UpdatedAt = DateTime.UtcNow;
        
        try
        {
            await _context.BoardPositions.AddAsync(boardPosition, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save boardPosition to database: {Message}", ex.Message);
            return Errors.UnexpectedError;
        }
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save boardPosition to database: {Message}", ex.Message);
            return Errors.UnexpectedError;
        }

        return new TurnGameResponse(boardPosition.PlayerGuid, boardPosition.GameId, boardPosition.RowIndex, boardPosition.ColIndex);
    }
    
    private void SwitchPlayerTurn(Domain.Entities.Game game)
    {
        if (game.PlayerTurn == game.PlayerOneId)
        {
            game.PlayerTurn = game.PlayerTwoId;
        }
        else if (game.PlayerTurn == game.PlayerTwoId)
        {
            game.PlayerTurn = game.PlayerOneId;
        }
    }
}