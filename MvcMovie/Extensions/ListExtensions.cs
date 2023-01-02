#region usings

#endregion

namespace MvcMovie.Extensions;

public static class ListExtensions
{
    /// <summary>
    ///     Moves the item matching the <paramref name="itemSelector" /> to the end of the <paramref name="list" />.
    /// </summary>
    public static void MoveToEnd<T>(this List<T> list, Predicate<T> itemSelector)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));

        if (list.Count > 1)
            list.Move(itemSelector, list.Count - 1);
    }

    /// <summary>
    ///     Moves the item matching the <paramref name="itemSelector" /> to the beginning of the <paramref name="list" />.
    /// </summary>
    public static void MoveToBeginning<T>(this List<T> list, Predicate<T> itemSelector)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        list.Move(itemSelector, 0);
    }

    /// <summary>
    ///     Moves the item matching the <paramref name="itemSelector" /> to the <paramref name="newIndex" /> in the
    ///     <paramref
    ///         name="list" />
    ///     .
    /// </summary>
    public static void Move<T>(this List<T> list, Predicate<T> itemSelector, int newIndex)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        if (itemSelector == null) throw new ArgumentNullException(nameof(itemSelector));
        if (newIndex < 0) throw new ArgumentOutOfRangeException(nameof(newIndex));

        var currentIndex = list.FindIndex(itemSelector);
        if (currentIndex < 0) throw new ArgumentException(nameof(currentIndex));

        if (currentIndex == newIndex)
            return;

        // Copy the item
        var item = list[currentIndex];

        // Remove the item from the list
        list.RemoveAt(currentIndex);

        // Finally insert the item at the new index
        list.Insert(newIndex, item);
    }

    public static IEnumerable<T[]> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToArray());
    }
}