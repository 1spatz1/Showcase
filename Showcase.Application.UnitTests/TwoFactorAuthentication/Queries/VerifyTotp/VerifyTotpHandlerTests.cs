using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OtpNet;
using Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;
using Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;
using Xunit.Abstractions;

namespace Showcase.Application.UnitTests.TwoFactorAuthentication.Queries.VerifyTotp;

public class VerifyTotpHandlerTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public VerifyTotpHandlerTests(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
    }

    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task VerifyTotpHandler_ShouldReturnSuccess()
    {
        DotNetEnv.Env.TraversePath().Load();
        // Arrange
        var logger = new Mock<ILogger<VerifyTotpQueryHandler>>();
        var configureTotplogger = new Mock<ILogger<ConfigureTotpCommandHandler>>();
        var handler = new VerifyTotpQueryHandler(logger.Object, _fixture.Context);

        // Get UserID
        Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());
        _testOutputHelper.WriteLine($"Guid of super-admin in the database: {UserId}");
        
        // Configure TOTP
        var configureTotpHandler = new ConfigureTotpCommandHandler(configureTotplogger.Object, _fixture.Context);
        var configureTotpCommand = new ConfigureTotpCommand(UserId);
        var configureTotpResult = await configureTotpHandler.Handle(configureTotpCommand, CancellationToken.None);
        _testOutputHelper.WriteLine(configureTotpResult.Value.SharedKey);
        
        var secretBytes = Base32Encoding.ToBytes(configureTotpResult.Value.SharedKey);
        var totp = new Totp(secretBytes);
        string token = totp.ComputeTotp();
        _testOutputHelper.WriteLine($"token: {token}");
        
        var command = new VerifyTotpQuery(token, UserId);
        _testOutputHelper.WriteLine($"command: {command}");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        _testOutputHelper.WriteLine($"result: {result}");

        // Assert
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        result.Value.Success.Should().BeTrue(); // Assert that the TOTP is enabled
    }
}