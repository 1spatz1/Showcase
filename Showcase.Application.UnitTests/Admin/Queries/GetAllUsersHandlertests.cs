using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Showcase.Application.Admin.Queries.GetUnlockedUsers;

namespace Showcase.Application.UnitTests.Admin.Queries;

public class GetAllUsersHandlertests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;
    
    public GetAllUsersHandlertests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task GetAllUsers_ReturnsUsers()
    {
        // Arrange
        var logger = new Mock<ILogger<GetAllUsersQueryHandler>>();
        var handler = new GetAllUsersQueryHandler(logger.Object, _fixture.Context);
        
        // Create GetAllUsersQuery
        var command = new GetAllUsersQuery();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        result.Value.UnlockedUsers.Should().NotBeEmpty(); // Assert that there are unlocked users
        result.Value.LockedUsers.Should().BeEmpty(); // Assert that there are no locked users
    }
}