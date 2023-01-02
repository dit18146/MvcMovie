#region usings

#endregion

namespace MvcMovie.Extensions.Linq;

/// <summary>
///     Provides a set of static methods for querying objects that
///     implement <see cref="IEnumerable{T}" />.
/// </summary>
public static partial class MoreEnumerable
{
    private static int? TryGetCollectionCount<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source is ICollection<T> collection ? collection.Count
            : source is IReadOnlyCollection<T> readOnlyCollection ? readOnlyCollection.Count
            : null;
    }

    private static int CountUpTo<T>(this IEnumerable<T> source, int max)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (max < 0)
            throw new ArgumentOutOfRangeException(nameof(max), "The maximum count argument cannot be negative.");

        var count = 0;

        using (var e = source.GetEnumerator())
        {
            while (count < max && e.MoveNext()) count++;
        }

        return count;
    }
}