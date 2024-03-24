using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;

namespace Showcase.Application.UnitTests.TwoFactorAuthentication.Commands.ConfigureTotp;

public class ConfigureTotpHandlerTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;

    public ConfigureTotpHandlerTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task ConfigureTotpHandler_ShouldCreateSecret()
    {
        DotNetEnv.Env.TraversePath().Load();
        // Arrange
        var logger = new Mock<ILogger<ConfigureTotpCommandHandler>>();
        var handler = new ConfigureTotpCommandHandler(logger.Object ,_fixture.Context);

        // Get UserID
        Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());

        var command = new ConfigureTotpCommand(UserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        result.Value.QrCodeUri.Should().NotBeNullOrEmpty(); // Assert that the QR code URI is not null or empty
        result.Value.SharedKey.Should().NotBeNullOrEmpty(); // Assert that the shared key is not null or empty
    }
}