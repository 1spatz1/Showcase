using ErrorOr;
using MediatR;
using Showcase.Application.Authentication.Common;

namespace Showcase.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password, string? Token = "") : IRequest<ErrorOr<AuthenticationResponse>>;