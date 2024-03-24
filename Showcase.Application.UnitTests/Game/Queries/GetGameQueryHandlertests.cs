using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Showcase.Application.Game.Commands.CreateGame;
using Showcase.Application.Game.Queries.GetGame;

namespace Showcase.Application.UnitTests.Game.Queries;

public class GetGameQueryHandlertests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;

    public GetGameQueryHandlertests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task GetGameQuery_ReturnsGameData()
    {
        // Arrange
        var logger = new Mock<ILogger<GetGameQueryHandler>>();
        var createGameLogger = new Mock<ILogger<CreateGameCommandHandler>>();
        var handler = new GetGameQueryHandler(logger.Object, _fixture.Context);
        
        // Get UserID
        Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());
        
        // Create Game
        var createGameHandler = new CreateGameCommandHandler(createGameLogger.Object, _fixture.Context);
        var createGameCommand = new CreateGameCommand(UserId);
        var createGameResult = await createGameHandler.Handle(createGameCommand, CancellationToken.None);
        var gameId = createGameResult.Value.GameId;
        
        // Create GetAllgamesQuery
        var command = new GetGameQuery(UserId, gameId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        result.Value.Game.Id.Should().Be(gameId); // Assert that the game ID matches the command's game ID
        result.Value.Game.PlayerOneId.Should().Be(UserId); // Assert that the game ID matches the command's game ID
        result.Value.Game.State.Should().Be(0); // Assert that the game ID matches the command's game ID
    }
}