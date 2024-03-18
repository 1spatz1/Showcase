using ErrorOr;
using MediatR;

namespace Showcase.Application.Game.Queries.GetGame;

public record GetGameQuery
(
    Guid UserId,
    string Username,
    Guid GameId
) : IRequest<ErrorOr<GetGameResponse>>;