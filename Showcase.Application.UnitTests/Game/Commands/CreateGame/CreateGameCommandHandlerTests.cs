using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Showcase.Application.Game.Commands.CreateGame;

namespace Showcase.Application.UnitTests.Game.Commands.CreateGame
{
    public class CreateGameCommandHandlerTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public CreateGameCommandHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.CreateNewContext(); // Create a new context for each test
        }

        [Fact]
        public async Task CreateGameCommandHandler_ShouldCreateGame()
        {
            // Arrange
            var logger = new Mock<ILogger<CreateGameCommandHandler>>();
            var handler = new CreateGameCommandHandler(logger.Object ,_fixture.Context);

            // Get UserID
            Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());

            var command = new CreateGameCommand(UserId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse(); // Assert that there are no errors
            var game = _fixture.Context.Games.Find(result.Value.GameId);
            game.Should().NotBeNull(); // Assert that the game was created in the database
            game.PlayerOneId.Should().Be(command.UserId); // Assert that the game's name matches the command's name
            // Add more assertions as needed
        }
    }
}