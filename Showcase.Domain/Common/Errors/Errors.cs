using ErrorOr;
namespace Showcase.Domain.Common.Errors;

public partial class Errors
{
    public static Error UnexpectedError => Error.Unexpected($"{nameof(Errors)}.{nameof(UnexpectedError)}", "An unexpected error occurred.");
}