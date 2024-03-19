using ErrorOr;
using MediatR;

namespace Showcase.Application.Authentication.Commands.LockUser;

public record LockUserCommand
(
    string Email,
    int DurationDays
) : IRequest<ErrorOr<LockUserResponse>>;