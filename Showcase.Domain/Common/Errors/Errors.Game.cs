using ErrorOr;

namespace Showcase.Domain.Common.Errors;

public static partial class Errors
{
    public static class Game
    {
        public static Error GameNotFound = Error.NotFound($"{nameof(Game)}.{nameof(GameNotFound)}",
            "Game not found");

        public static Error GameNotInProgress = Error.Conflict($"{nameof(Game)}.{nameof(GameNotInProgress)}",
            "Game not in progress");

        public static Error GameNotYourTurn = Error.Conflict($"{nameof(Game)}.{nameof(GameNotYourTurn)}",
            "It's not your turn");

        public static Error GameAlreadyOver = Error.Conflict($"{nameof(Game)}.{nameof(GameAlreadyOver)}",
            "Game already over");

        public static Error InvalidMove = Error.Conflict($"{nameof(Game)}.{nameof(InvalidMove)}",
            "Invalid move");
    }
}