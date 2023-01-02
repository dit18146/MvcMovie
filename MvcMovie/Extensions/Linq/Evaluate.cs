#region usings

#endregion

namespace MvcMovie.Extensions.Linq;

public static partial class MoreEnumerable
{
    /// <summary>
    ///     Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of
    ///     functions.
    /// </summary>
    /// <remarks>
    ///     This operator uses deferred execution and streams the results.
    ///     If the resulting sequence is enumerated multiple times, the functions will be
    ///     evaluated multiple times too.
    /// </remarks>
    /// <typeparam name="T">The type of the object returned by the functions.</typeparam>
    /// <param name="functions">The functions to evaluate.</param>
    /// <returns>A sequence with results from invoking <paramref name="functions" />.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="functions" /> is <c>null</c>.</exception>
    public static IEnumerable<T> Evaluate<T>(this IEnumerable<Func<T>> functions) =>
        from f in functions ?? throw new ArgumentNullException(nameof(functions))
        select f();
}