using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Application.Game.Commands.JoinGame;
using Showcase.Application.Game.Commands.placeTurn;
using Showcase.Application.Game.Commands.TurnGame;

namespace Showcase.Application.UnitTests.Game.Commands.TurnGame;

public class TurnGameCommandHandlerTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;

    public TurnGameCommandHandlerTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task TurnGameCommandHandler_ShouldTurnGame()
    {
        // Arrange
        var logger = new Mock<ILogger<TurnGameCommandHandler>>();
        var joinGameLogger = new Mock<ILogger<JoinGameCommandHandler>>();
        var createGameLogger = new Mock<ILogger<CreateGameCommandHandler>>();
        var handler = new TurnGameCommandHandler(logger.Object, _fixture.Context);

        // Get UserID
        Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());
        Guid testUserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "test-user").Select(x => x.Id).FirstOrDefault().ToString());

        // Create Game
        var createGameCommand = new CreateGameCommand(UserId);
        var createGameHandler = new CreateGameCommandHandler(createGameLogger.Object, _fixture.Context);
        var createGameResult = await createGameHandler.Handle(createGameCommand, CancellationToken.None);
        Guid gameId = createGameResult.Value.GameId;
        
        // Join Game
        var joinGameCommand = new JoinGameCommand(testUserId, gameId);
        var joinGameHandler = new JoinGameCommandHandler(joinGameLogger.Object, _fixture.Context);
        var joinGameResult = await joinGameHandler.Handle(joinGameCommand, CancellationToken.None);
        
        // var gameData = await _fixture.Context.Games.FindAsync(gameId);
        var gameData = await _fixture.Context.Games.FirstOrDefaultAsync(x => x.Id == gameId, CancellationToken.None);
        Guid playerTurn = (Guid)gameData.PlayerTurn;

        // Create TurnGameCommand
        var command = new TurnGameCommand(playerTurn, gameId, 1, 1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        var game = _fixture.Context.Games.Include(x => x.Board).FirstOrDefault(x => x.Id == gameId);
        game.Board.Should().NotBeEmpty(); // Assert that the game has board positions
        game.PlayerTurn.Should().NotBe(playerTurn); // Assert that the player turn has changed
        game.Board!.FirstOrDefault(x => x.RowIndex == 1 && x.ColIndex == 1).Should().NotBeNull(); // Assert that the board position was created
    }
}