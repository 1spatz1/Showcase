using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Game.Queries.CheckGameStatus;

public class CheckGameStatusQueryHandler : IRequestHandler<CheckGameStatusQuery, ErrorOr<CheckGameStatusResponse>>
{
    private readonly ILogger<CheckGameStatusQuery> _logger;
    private readonly IApplicationDbContext _context;
    
    public CheckGameStatusQueryHandler(ILogger<CheckGameStatusQuery> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<ErrorOr<CheckGameStatusResponse>> Handle(CheckGameStatusQuery request, CancellationToken cancellationToken)
{
    // get the game with board
    var game = await _context.Games.Include(x => x.Board).FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);

    // Check if Game exists
    if (game is null)
        return Errors.Game.GameNotFound;

    // Check if the game is already over
    if (game.State is GameState.Over or GameState.Draw or GameState.Cancelled) 
        return Errors.Game.GameAlreadyOver;

    // Check if the row is full and all positions are filled by the same user
    if (game.Board!.Count(bp => bp.RowIndex == request.RowIndex && bp.PlayerGuid == request.UserId) == game.BoardSize)
        return new CheckGameStatusResponse(request.UserId, request.GameId, request.RowIndex, request.ColIndex, true, false);

    // Check if the col is full and all positions are filled by the same user
    if (game.Board!.Count(bp => bp.ColIndex == request.ColIndex && bp.PlayerGuid == request.UserId) == game.BoardSize)
        return new CheckGameStatusResponse(request.UserId, request.GameId, request.RowIndex, request.ColIndex, true, false);

    // Check if the diagonal is full and all positions are filled by the same user
    if (request.RowIndex == request.ColIndex && game.Board!.Count(bp => bp.RowIndex == bp.ColIndex && bp.PlayerGuid == request.UserId) == game.BoardSize)
        return new CheckGameStatusResponse(request.UserId, request.GameId, request.RowIndex, request.ColIndex, true, false);

    // Check if the anti-diagonal is full and all positions are filled by the same user
    if (request.RowIndex + request.ColIndex == game.BoardSize - 1 && game.Board!.Count(bp => bp.RowIndex + bp.ColIndex == game.BoardSize - 1 && bp.PlayerGuid == request.UserId) == game.BoardSize)
        return new CheckGameStatusResponse(request.UserId, request.GameId, request.RowIndex, request.ColIndex, true, false);

    // Check if all positions on the board are filled (i.e., the game is a draw)
    if(game.Board!.Count == game.BoardSize * game.BoardSize)
        return new CheckGameStatusResponse(request.UserId, request.GameId, request.RowIndex, request.ColIndex, false, true);

    return new CheckGameStatusResponse(request.UserId, request.GameId, request.RowIndex, request.ColIndex, false, false);
}
}