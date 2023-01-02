#region usings

#endregion

namespace MvcMovie.Extensions;

/// <summary>
///     Extension methods for <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" />
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    ///     Gets the value associated with the specified key or the <paramref name="defaultValue" /> if it does not exist.
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key">The key whose value to get.</param>
    /// <param name="defaultValue">
    ///     The default value to return if an item with the specified <paramref name="key" /> does not exist.
    /// </param>
    /// <returns>
    ///     The value associated with the specified key or the <paramref name="defaultValue" /> if it does not exist.
    /// </returns>
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        TKey key, TValue? defaultValue = default) =>
        dictionary.TryGetValue(key, out var value) ? value : defaultValue;

    /// <summary>
    ///     Adds all elements from source to target
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="source">Source dictionary to get the values from</param>
    /// <param name="target">Target dictionary to add values to</param>
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue>? target,
        IDictionary<TKey, TValue>? source)
    {
        if (target == null || source == null) return;

        foreach (var pair in source) target.Add(pair);
    }

    /// <summary>
    ///     Gets element by key if it exists in the dictionary, otherwise calls specifed method to
    ///     create a new element and adds it back to the dictionary
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="target">Target dictionary</param>
    /// <param name="key">Key to search on</param>
    /// <param name="createValue">Method used to create a new value</param>
    /// <returns></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> target,
        TKey key,
        Func<TValue> createValue)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (createValue == null) throw new ArgumentNullException(nameof(createValue));

        if (target.TryGetValue(key, out var value)) return value;

        value = createValue();
        target[key] = value;

        return value;
    }
}