using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace MvcMovie.Extensions;

/// <summary>
///     Extension methods for <see cref="System.Collections.Generic.IEnumerable{T}" />
/// </summary>
public static class EnumerableExtensions
{
    ///// <summary>
    ///// Performs a specific action on each element of the sequence
    ///// </summary>
    //public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    //{
    //    if (source == null) throw new ArgumentNullException(nameof(source));
    //    if (action == null) throw new ArgumentNullException(nameof(action));

    //    foreach (T element in source)
    //    {
    //        action(element);

    //        yield return element;
    //    }
    //}

    /// <summary>
    ///     Performs an action on each value of the enumerable
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="source">Sequence on which to perform action</param>
    /// <param name="action">Action to perform on every item</param>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown when given null <paramref name="source" /> or <paramref name="action" />
    /// </exception>
    public static void ForEach<T>(this IEnumerable<T>? source, Action<T>? action)
    {
        if (source == null || action == null)
            return;

        foreach (T value in source)
            action(value);
    }

    /// <summary>
    ///     ICollection extension brining the AddRange from List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="source"></param>
    public static void AddRange<T>(this ICollection<T>? collection, IEnumerable<T>? source)
    {
        if (collection == null) return;
        if (source == null) return;

        foreach (T element in source) collection.Add(element);
    }


    /// <summary>
    ///     Convenience method for retrieving a specific page of items within a collection.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageIndex">The index of the page to get.</param>
    /// <param name="pageSize">The size of the pages.</param>
    public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (pageIndex < 0) throw new ArgumentOutOfRangeException(nameof(pageIndex));
        if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

        return source.Skip(pageIndex * pageSize).Take(pageSize);
    }

    /// <summary>
    ///     Generic Extension for simple paging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetPagedData<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var skipRows = pageNumber == 1 ? 0 : (pageNumber - 1) * pageSize;
        return source.Skip(skipRows).Take(pageSize);
    }

    /// <summary>
    ///     Converts an enumerable into a readonly collection
    /// </summary>
    public static IEnumerable<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable) =>
        new ReadOnlyCollection<T>(enumerable.ToList());

    /// <summary>
    ///     Validates that the <paramref name="enumerable" /> is not null and contains items.
    /// </summary>
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? enumerable) => enumerable != null && enumerable.Any();

    /// <summary>
    ///     Concatenates the members of a collection, using the specified separator between each member.
    /// </summary>
    /// <returns>
    ///     A string that consists of the members of <paramref name="values" /> delimited by the <paramref name="separator" />
    ///     string. If values has no members, the method returns null.
    /// </returns>
    public static string? JoinOrDefault<T>(this IEnumerable<T>? values, string separator)
    {
        if (separator == null) throw new ArgumentNullException(nameof(separator));

        return values == null ? default : string.Join(separator, values);
    }

    public static string GetDescription<T>(this T e) where T : IConvertible
    {
        switch (e)
        {
            case Enum:
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val) ?? string.Empty);

                        if (memInfo[0]
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                            return descriptionAttribute.Description;
                    }

                break;
            }
        }

        return string.Empty;
    }
}