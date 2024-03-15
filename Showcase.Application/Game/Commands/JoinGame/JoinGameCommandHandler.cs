using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Game.Commands.JoinGame;

public class JoinGameCommandHandler : IRequestHandler<JoinGameCommand, ErrorOr<JoinGameResponse>>
{
   private readonly ILogger<JoinGameCommandHandler> _logger;
   private readonly IApplicationDbContext _context;

   public JoinGameCommandHandler(ILogger<JoinGameCommandHandler> logger, IApplicationDbContext context)
   {
      _logger = logger;
      _context = context;
   }

   public async Task<ErrorOr<JoinGameResponse>> Handle(JoinGameCommand request, CancellationToken cancellationToken)
   {
      // Get the game with board
      var game = await _context.Games.Include(g => g.Board)
         .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);
      
      // Check if Game exists
      if (game is null)
         return Errors.Game.GameNotFound;
      
      // Check if playertwo is not empty & playerid is not equal to any of playerid's
      if (game.PlayerTwoId != null && game.PlayerOneId != request.UserId && game.PlayerOneId != request.UserId)
         return Errors.Authorisation.NotAllowed;
      
      // Check if the game is already over
      if (game.State is GameState.Over or GameState.Draw or GameState.Cancelled) 
         return Errors.Game.GameAlreadyOver;
      
      // Check if playertwo is empty & if empty update the game with player two id and update in the database
      if (game.PlayerTwoId == null)
      {
         Random random = new Random();
         int randomNumber = random.Next(0, 2);

         game.PlayerTurn = randomNumber == 0 ? game.PlayerOneId : request.UserId;
         game.PlayerTwoId = request.UserId;
         game.State = GameState.InProgress;
         game.UpdatedAt = DateTime.UtcNow;

         try
         {
            await _context.SaveChangesAsync(cancellationToken);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, "Failed to update game with PlayerTwoId in database: {Message}", ex.Message);
            return Errors.UnexpectedError;
         }
         return new JoinGameResponse(request.UserId, request.Username, request.Token, game);
      }
      
      // Check if request Player is PlayerOne or PlayerTwo
      if (request.UserId == game.PlayerOneId || request.UserId == game.PlayerTwoId)
         return new JoinGameResponse(request.UserId, request.Username, request.Token, game);

      return Errors.UnexpectedError;
   }
}