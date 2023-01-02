#region usings

#endregion

namespace MvcMovie.Extensions.Linq;

public static partial class MoreEnumerable
{
    /// <summary>
    ///     Asserts that all elements of a sequence meet a given condition
    ///     otherwise throws an <see cref="Exception" /> object.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source" /> sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="predicate">Function that asserts an element of the <paramref name="source" /> sequence for a condition.</param>
    /// <returns>
    ///     Returns the original sequence.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     The input sequence
    ///     contains an element that does not meet the condition being
    ///     asserted.
    /// </exception>
    /// <remarks>
    ///     This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IEnumerable<TSource> Assert<TSource>(this IEnumerable<TSource> source,
        Func<TSource, bool> predicate) =>
        Assert(source, predicate, null);

    /// <summary>
    ///     Asserts that all elements of a sequence meet a given condition
    ///     otherwise throws an <see cref="Exception" /> object.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in <paramref name="source" /> sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="predicate">Function that asserts an element of the input sequence for a condition.</param>
    /// <param name="errorSelector">Function that returns the <see cref="Exception" /> object to throw.</param>
    /// <returns>
    ///     Returns the original sequence.
    /// </returns>
    /// <remarks>
    ///     This operator uses deferred execution and streams its results.
    /// </remarks>
    public static IEnumerable<TSource> Assert<TSource>(this IEnumerable<TSource> source,
        Func<TSource, bool> predicate, Func<TSource, Exception>? errorSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return _();

        IEnumerable<TSource> _()
        {
            foreach (var element in source)
            {
                var success = predicate(element);
                if (!success)
                    throw errorSelector?.Invoke(element) ??
                          new InvalidOperationException("Sequence contains an invalid item.");
                yield return element;
            }
        }
    }
}