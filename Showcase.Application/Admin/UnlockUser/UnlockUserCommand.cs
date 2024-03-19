using ErrorOr;
using MediatR;

namespace Showcase.Application.Authentication.Commands.UnlockUser;

public record UnlockUserCommand
(
    string Email
) : IRequest<ErrorOr<UnlockUserResponse>>;