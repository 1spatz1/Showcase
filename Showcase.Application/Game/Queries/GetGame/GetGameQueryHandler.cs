using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Game.Queries.GetGame;

public class GetGameQueryHandler : IRequestHandler<GetGameQuery, ErrorOr<GetGameResponse>>
{
    private readonly ILogger<GetGameQueryHandler> _logger;
    private readonly IApplicationDbContext _context;

    public GetGameQueryHandler(ILogger<GetGameQueryHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ErrorOr<GetGameResponse>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        // Get the game with board
        var game = await _context.Games.Include(g => g.Board)
            .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);
        
        // Check if Game exists
        if (game is null)
            return Errors.Game.GameNotFound;
        
        // Check if playerid is not equal to any of playerid's
        if (game.PlayerOneId != request.UserId && game.PlayerOneId != request.UserId)
            return Errors.Authorisation.NotAllowed;

        return new GetGameResponse(request.UserId, request.Username, request.Token, game);
    }
}