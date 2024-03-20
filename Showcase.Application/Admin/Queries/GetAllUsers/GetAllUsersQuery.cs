using ErrorOr;
using MediatR;

namespace Showcase.Application.Admin.Queries.GetUnlockedUsers;

public record GetAllUsersQuery : IRequest<ErrorOr<GetAllUsersResponse>>;