using ErrorOr;
using MediatR;

namespace Showcase.Application.Authentication.Commands.LockUser;

public record LockUserCommand
(
    string Email,
    string DurationDays
) : IRequest<ErrorOr<LockUserResponse>>;