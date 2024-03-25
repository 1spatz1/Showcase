using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OtpNet;
using Showcase.Application.Common.Mapping;
using Showcase.Application.TwoFactorAuthentication.Commands.ConfigureTotp;
using Showcase.Application.TwoFactorAuthentication.Commands.EnableTotp;
using Showcase.Application.TwoFactorAuthentication.Queries.VerifyTotp;
using Showcase.Domain.Common.Errors;
using Xunit.Abstractions;

namespace Showcase.Application.UnitTests.TwoFactorAuthentication.Commands.EnableTotp;

public class EnableTotpHandlerTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _testOutputHelper;
    private IMapper _mapper;

    public EnableTotpHandlerTests(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
    }

    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task EnableTotpHandler_ShouldReturnSuccess()
    {
        DotNetEnv.Env.TraversePath().Load();
        // Arrange;
        var logger = new Mock<ILogger<EnableTotpCommandHandler>>();
        var configureTotplogger = new Mock<ILogger<ConfigureTotpCommandHandler>>();
        var verifyTotplogger = new Mock<ILogger<VerifyTotpQueryHandler>>();
        var handler = new EnableTotpCommandHandler(logger.Object, _fixture.Context);

        // Get UserID
        Guid UserId = Guid.Parse(_fixture.Context.Users.Where(x => x.UserName == "super-admin").Select(x => x.Id).FirstOrDefault().ToString());
        _testOutputHelper.WriteLine($"Guid of super-admin in the database: {UserId}");
        
        // Configure TOTP
        var configureTotpHandler = new ConfigureTotpCommandHandler(configureTotplogger.Object, _fixture.Context);
        var configureTotpCommand = new ConfigureTotpCommand(UserId);
        var configureTotpResult = await configureTotpHandler.Handle(configureTotpCommand, CancellationToken.None);
        _testOutputHelper.WriteLine(configureTotpResult.Value.SharedKey);
        
        // make TOTP token
        var secretBytes = Base32Encoding.ToBytes(configureTotpResult.Value.SharedKey);
        var totp = new Totp(secretBytes);
        string token = totp.ComputeTotp();
        
        // Verify TOTP
        var verifyTotpHandler = new VerifyTotpQueryHandler(verifyTotplogger.Object, _fixture.Context);
        var verifyTotpCommand = new VerifyTotpQuery(token, UserId);
        var verifyTotpResult = await verifyTotpHandler.Handle(verifyTotpCommand, CancellationToken.None);
        _testOutputHelper.WriteLine(verifyTotpResult.Value.Success.ToString());
        
        var command = new EnableTotpCommand(UserId);
        _testOutputHelper.WriteLine($"command: {command}");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        _testOutputHelper.WriteLine($"result: {result}");

        // Assert
        verifyTotpResult.Value.Success.Should().BeTrue(); // Assert that the TOTP is successfully verified
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        result.Value.Success.Should().Be(true); // Assert that the TOTP is enabled
    }
}