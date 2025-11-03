using FluentResults;

namespace Weather.API.Domain.Extensions
{
    public static class FluentResultExtensions
    {
        public static IEnumerable<string> ToErrorMessages(this IReadOnlyList<IError> errors)
        {
            if (!errors.HasAny())
            {
                return Array.Empty<string>();
            }

            return errors.Select(error => error.Message);
        }

        public static string JoinToMessage(this IReadOnlyList<IError> errors)
        {
            if (!errors.HasAny())
            {
                return string.Empty;
            }

            return string.Join(',', errors.ToErrorMessages());
        }
    }
}
