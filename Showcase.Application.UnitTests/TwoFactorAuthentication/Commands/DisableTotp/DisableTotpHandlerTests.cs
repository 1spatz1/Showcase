using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.DisableTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;
using Showcase.Domain.Entities;
using Xunit.Abstractions;

namespace Showcase.Application.UnitTests.TwoFactorAuthentication.Commands.DisableTotp;

public class DisableTotpHandlerTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public DisableTotpHandlerTests(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
    }

    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task DisableTotpHandlerTests_ShouldReturnSuccess()
    {
        DotNetEnv.Env.TraversePath().Load();
        // Arrange
        var logger = new Mock<ILogger<DisableTotpCommandHandler>>();
        var configureTotplogger = new Mock<ILogger<ConfigureTotpCommandHandler>>();
        var handler = new DisableTotpCommandHandler(logger.Object, _fixture.Context);

        // Get UserID
        Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());
        _testOutputHelper.WriteLine($"Guid of super-admin in the database: {UserId}");
        
        // Configure TOTP
        var configureTotpHandler = new ConfigureTotpCommandHandler(configureTotplogger.Object, _fixture.Context);
        var configureTotpCommand = new ConfigureTotpCommand(UserId);
        var configureTotpResult = await configureTotpHandler.Handle(configureTotpCommand, CancellationToken.None);
        _testOutputHelper.WriteLine(configureTotpResult.Value.SharedKey);
        
        
        var command = new DisableTotpCommand(UserId);
        _testOutputHelper.WriteLine($"command: {command}");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        _testOutputHelper.WriteLine($"result: {result}");

        // Assert
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        result.Value.Success.Should().Be(true); // Assert that the TOTP is enabled
    }
}