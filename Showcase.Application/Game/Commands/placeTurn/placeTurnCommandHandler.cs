using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Game.Commands.placeTurn;

public class PlaceTurnCommandHandler : IRequestHandler<placeTurnCommand, ErrorOr<placeTurnCommandResponse>>
{
    private readonly ILogger<CreateGameCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public PlaceTurnCommandHandler(ILogger<CreateGameCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<ErrorOr<placeTurnCommandResponse>> Handle(placeTurnCommand request, CancellationToken cancellationToken)
    {
        // Check if Player is not the current turn
        if (!await _context.Games.AnyAsync(x => x.Id == request.GameId && x.PlayerTurn == request.UserId,
                cancellationToken))
            return Errors.UnexpectedError;
        
        // Check if the position is already taken
        if(await _context.BoardPositions.AnyAsync(bp => bp.GameId == request.GameId && bp.RowIndex == request.RowIndex && bp.ColIndex == request.ColIndex,
            cancellationToken))
            return Errors.UnexpectedError;
        
        BoardPosition boardPosition = new()
        {
            Id = Guid.NewGuid(),
            PlayerGuid = request.UserId,
            GameId = request.GameId,
            RowIndex = request.RowIndex,
            ColIndex = request.ColIndex,
            CreatedAt = DateTime.UtcNow
        };
        
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

        return new placeTurnCommandResponse(boardPosition.PlayerGuid, boardPosition.GameId, boardPosition.RowIndex, boardPosition.ColIndex);
    }
}