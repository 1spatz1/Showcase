using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Showcase.Application.Game.Commands.JoinGame;
using Showcase.Domain.Entities;
using Showcase.Application.Game.Commands.CreateGame;
using Microsoft.AspNetCore.Identity;
using Showcase.Application.Authentication.Commands.Register;
using Showcase.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Showcase.Application.UnitTests.Game.Commands.JoinGame
{
    public class JoinGameCommandHandlerTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public JoinGameCommandHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.CreateNewContext(); // Create a new context for each test
        }

        [Fact]
        public async Task JoinGameCommandHandler_ShouldJoinGame()
        {
            // Arrange
            var logger = new Mock<ILogger<JoinGameCommandHandler>>();
            var createGameLogger = new Mock<ILogger<CreateGameCommandHandler>>();
            var handler = new JoinGameCommandHandler(logger.Object, _fixture.Context);

            // Get UserID
            Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());

            // Create Game
            var createGameHandler = new CreateGameCommandHandler(createGameLogger.Object, _fixture.Context);
            var createGameCommand = new CreateGameCommand(UserId);
            var createGameResult = await createGameHandler.Handle(createGameCommand, CancellationToken.None);
            var gameId = createGameResult.Value.GameId;

            // Get UserID
            Guid testUserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "test-user").Select(x => x.Id).FirstOrDefault().ToString());

            // Create JoinGameCommand
            var command = new JoinGameCommand(testUserId, gameId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse(); // Assert that there are no errors
            var game = await _fixture.Context.Games.FindAsync(gameId);
            game.Should().NotBeNull(); // Assert that the game exists
            game.PlayerTwoId.Should().Be(command.UserId); // Assert that the second player of the game matches the command's user ID
            game.PlayerTurn.Should().NotBeNull();
        }
    }
}