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
        Setup();
    }
    
    private void Setup()
    {
        // Create a new service collection
        var serviceCollection = new ServiceCollection();

        // Add your mappings
        serviceCollection.AddMapping(); // Replace with your actual mapping configuration

        // Build the service provider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve the IMapper instance
        _mapper = serviceProvider.GetService<IMapper>();
    }

    public void Dispose()
    {
        _fixture.CreateNewContext(); // Create a new context for each test
    }

    [Fact]
    public async Task EnableTotpHandler_ShouldReturnSuccess()
    {
        DotNetEnv.Env.TraversePath().Load();
        // Arrange
        var mediator = new Mock<IMediator>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<EnableTotpCommandHandler>>();
        var configureTotplogger = new Mock<ILogger<ConfigureTotpCommandHandler>>();
        var handler = new EnableTotpCommandHandler(logger.Object, _fixture.Context, mediator.Object, _mapper);

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
        
        var command = new EnableTotpCommand(token, UserId);
        _testOutputHelper.WriteLine($"command: {command}");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        _testOutputHelper.WriteLine($"result: {result}");

        // Assert
        result.IsError.Should().BeFalse(); // Assert that there are no errors
        result.Value.Success.Should().Be(true); // Assert that the TOTP is enabled
    }
}