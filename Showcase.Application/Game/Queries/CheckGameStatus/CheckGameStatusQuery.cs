using ErrorOr;
using MediatR;

namespace Showcase.Application.Game.Queries.CheckGameStatus;

public record CheckGameStatusQuery
(
    Guid UserId,
    Guid GameId,
    int RowIndex,
    int ColIndex
) : IRequest<ErrorOr<CheckGameStatusResponse>>;