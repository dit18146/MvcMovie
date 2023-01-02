using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcMovie.Extensions;

/// <summary>
///     Provides formatting of strings using object properties.
/// </summary>
[DebuggerStepThrough]
public static class StringExtensions
{
    private const string _htmlStripPattern = @"<(.|\n)*?>";

    private static readonly char[] _invalid = Path.GetInvalidFileNameChars();

    /// <summary>
    ///     Converts the specified value to a <see cref="bool" />
    /// </summary>
    public static bool ToBool(this string value, bool defaultValue = false) =>
        bool.TryParse(value, out bool parsed) ? parsed : defaultValue;

    /// <summary>
    ///     Whether the specified value is string representation of a <see cref="DateTime" />
    /// </summary>
    public static bool IsDateTime(this string value) => DateTime.TryParse(value, out DateTime dateValue);

    /// <summary>
    ///     Whether the specified value is a string representation of a <see cref="Guid" />.
    /// </summary>
    public static bool IsGuid(this string value) => Guid.TryParse(value, out Guid guid);

    /// <summary>
    ///     Whether the specified value in not null and not empty
    /// </summary>
    public static bool HasValue(this string? value) => !string.IsNullOrEmpty(value);

    /// <summary>
    ///     Whether the specified value is exactly equal to other value
    /// </summary>
    public static bool EqualsOrdinal(this string value, string other) =>
        string.Equals(value, other, StringComparison.Ordinal);

    /// <summary>
    ///     Whether the specified value is not exactly equal to other value
    /// </summary>
    public static bool NotEqualsOrdinal(this string value, string other) => !value.EqualsOrdinal(other);

    ///// <summary>
    /////     Whether the specified value is not equal to other value
    ///// </summary>
    //public static bool NotEqualsIgnoreCase(this string value, string other)
    //{
    //    return !value.EqualsIgnoreCase(other);
    //}


    /// <summary>
    ///     Makes a string camel cased.
    /// </summary>
    /// <param name="identifier"> The identifier to camel case </param>
    public static string MakeCamel(this string identifier)
    {
        if (identifier.Length <= 2)
            return identifier.ToLowerInvariant();

        if (char.IsUpper(identifier[0]))
            return char.ToLowerInvariant(identifier[0]) + identifier.Substring(1);

        return identifier;
    }

    /// <summary>
    ///     Whether the <see cref="formattedString" /> has been formatted from the specified <see cref="formatString" />.
    /// </summary>
    /// <remarks>
    ///     This function is useful for comparing two strings where the <see cref="formattedString" /> is the result of a
    ///     String.Format operation on
    ///     the <see cref="formatString" />, with one or more format substitutions.
    ///     For example: Calling this function with a string "My code is 5" and a resource string "My code is {0}" that
    ///     contains one or more formatting arguments, return
    ///     <c> true </c>
    /// </remarks>
    public static bool IsFormattedFrom(this string formattedString, string formatString)
    {
        var escapedPattern = formatString
            .Replace("[", "\\[")
            .Replace("]", "\\]")
            .Replace("(", "\\(")
            .Replace(")", "\\)")
            .Replace(".", "\\.")
            .Replace("<", "\\<")
            .Replace(">", "\\>");

        var pattern = Regex.Replace(escapedPattern, @"\{\d+\}", ".*")
            .Replace(" ", @"\s");

        return new Regex(pattern).IsMatch(formattedString);
    }

    /// <summary>
    ///     Ensures the specified path has no trailing slash.
    /// </summary>
    public static string WithoutTrailingSlash(this string path) => path.Trim('/');

    /// <summary>
    ///     Ensures that the specified path has a leading slash
    /// </summary>
    public static string WithLeadingSlash(this string path) => $"/{path.TrimStart('/')}";

    /// <summary>
    ///     Returns an array of values split from the specified string, by the specified delimiters
    /// </summary>
    public static string[] SafeSplit(this string value, char[] delimiters,
        StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
    {
        if (!value.HasValue())
            return new string[]
            {
            };

        return value.Split(delimiters, options);
    }

    /// <summary>
    ///     Returns an array of values split from the specified string, by the specified delimiters
    /// </summary>
    public static string[] SafeSplit(this string value, string[] delimiters,
        StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
    {
        if (!value.HasValue())
            return new string[]
            {
            };

        return value.Split(delimiters, options);
    }

    /// <summary>
    ///     Returns an array of values split from the specified string, by the specified delimiters
    /// </summary>
    public static string[] SafeSplit(this string value, string delimiter,
        StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
    {
        return value.SafeSplit(new[]
        {
            delimiter
        }, options);
    }

    #region ToUpperLowerNameVariant

    /// <summary>
    ///     Converts string to a Name-Format where each first letter is Uppercase.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns></returns>
    public static string ToUpperLowerNameVariant(this string value)
    {
        if (value.IsNullOrEmptyOrWhiteSpace()) return string.Empty;

        char[] valuearray = value.ToLower().ToCharArray();

        bool nextupper = true;

        for (int i = 0; i < valuearray.Count() - 1; i++)
            if (nextupper)
            {
                valuearray[i] = char.Parse(valuearray[i].ToString().ToUpper());
                nextupper = false;
            }
            else
            {
                nextupper = valuearray[i] switch
                {
                    ' ' or '-' or '.' or ':' or '\n' => true,
                    _ => false
                };
            }

        return new string(valuearray);
    }

    #endregion

    #region IsValidUrl

    /// <summary>
    ///     Determines whether it is a valid URL.
    /// </summary>
    /// <returns>
    ///     <c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
    /// </returns>
    /*public static bool IsValidUrl(this string url)
    {
        Regex rx = RegexValidationHelper.UrlRegex;
        return rx.IsMatch(url);
    }*/

    #endregion

    #region Truncate

    /// <summary>
    ///     Truncates the string to a specified length and replace the truncated to a ...
    /// </summary>
    /// <param name="text"></param>
    /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
    /// <returns>truncated string</returns>
    public static string Truncate(this string text, int maxLength)
    {
        // replaces the truncated string to a ...
        const string suffix = "...";
        string truncatedString = text;

        if (maxLength <= 0) return truncatedString;
        int strLength = maxLength - suffix.Length;

        if (strLength <= 0) return truncatedString;

        if (text == null || text.Length <= maxLength) return truncatedString;

        truncatedString = text.Substring(0, strLength);
        truncatedString = truncatedString.TrimEnd();
        truncatedString += suffix;
        return truncatedString;
    }

    #endregion

    #region IsNullOrEmpty

    /// <summary>
    ///     A nicer way of calling <see cref="System.String.IsNullOrEmpty(string)" />
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmptyOrWhiteSpace(this string? value) =>
        string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

    /// <summary>
    ///     A nicer way of calling the inverse of <see cref="System.String.IsNullOrEmpty(string)" />
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>true if the value parameter is not null or an empty string (""); otherwise, false.</returns>
    public static bool IsNotNullOrEmptyOrWhiteSpace(this string value) => !value.IsNullOrEmptyOrWhiteSpace();

    #endregion

    /// <summary>
    ///     Separates a PascalCase string
    /// </summary>
    /// <example>
    ///     "ThisIsPascalCase".SeparatePascalCase(); // returns "This Is Pascal Case"
    /// </example>
    /// <param name="value">The value to split</param>
    /// <returns>The original string separated on each uppercase character.</returns>
    public static string SeparatePascalCase(this string value)
    {
        if (value.IsNotNullOrEmpty()) throw new ArgumentNullException(nameof(value));

        return Regex.Replace(value, "([A-Z])", " $1").Trim();
    }


    /// <summary>
    ///     Greek capital letters to replace
    /// </summary>
    private static readonly Dictionary<string, string> _greekAccentedCapitalsForReplace = new Dictionary
        <string, string>
        {
            { "Ά", "Α" },
            { "Έ", "Ε" },
            { "Ή", "Η" },
            { "Ί", "Ι" },
            { "Ό", "Ο" },
            { "Ύ", "Υ" },
            { "Ώ", "Ω" }
        };


    /// <summary>
    ///     Transforms input to capital letters replacing accented capitals
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToCapitalized(this string input)
    {
        if (input.IsNullOrEmptyOrWhiteSpace())
            return string.Empty;

        var sb = new StringBuilder(input.ToUpper(Thread.CurrentThread.CurrentCulture), input.Length);

        foreach (string key in _greekAccentedCapitalsForReplace.Keys)
            sb.Replace(key, _greekAccentedCapitalsForReplace[key]);

        return sb.ToString();
    }

    /// <summary>
    ///     URL-encodes input string
    /// </summary>
    /// <param name="value">String to encode</param>
    /// <returns>Encoded string</returns>
    public static string UrlEncode(this string value) => WebUtility.UrlEncode(value);

    /// <summary>
    ///     URL-decodes input string
    /// </summary>
    /// <param name="value">String to decode</param>
    /// <returns>Decoded string</returns>
    public static string UrlDecode(this string value) => WebUtility.UrlDecode(value);

    /// <summary>
    ///     Removes accents from string
    ///     non language dependent
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToNonAccent(this string value)
    {
        var normalizedString = value.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark) stringBuilder.Append(c);
        }

        var accentFreeString = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        return accentFreeString;
    }

    #region [ Stream Conversion ]

    /// <summary>
    ///     Converts to MemoryStream with a specific encoding
    /// </summary>
    public static MemoryStream? ToMemoryStream(this string? s, Encoding? encoding)
    {
        if (s == null) return null;

        encoding ??= Encoding.UTF8;

        return new MemoryStream(encoding.GetBytes(s));
    }

    /// <summary>
    ///     Converts to MemoryStream in UTF-8 encoding
    /// </summary>
    public static MemoryStream? ToMemoryStream(this string? s) =>
        // ReSharper disable once IntroduceOptionalParameters.Global
        ToMemoryStream(s, null);

    #endregion


    #region [ Filesystem ]

    /// <summary>
    ///     Removes invalid path characters from the string, replacing them by ('_') character
    /// </summary>
    public static string SanitizePath(this string path) =>
        // ReSharper disable once IntroduceOptionalParameters.Global
        SanitizePath(path, '_');

    /// <summary>
    ///     Removes invalid path characters from the string, replacing them by the given character
    /// </summary>
    public static string SanitizePath(this string path, char replacement)
    {
        if (_invalid.Contains(replacement))
            throw new ArgumentException("replacement char " + replacement + " is not a valid path char");

        if (string.IsNullOrEmpty(path)) return path;

        int first = path.IndexOfAny(_invalid);
        if (first == -1) return path;

        int length = path.Length;
        var result = new StringBuilder(length);
        result.Append(path, 0, first);
        result.Append(replacement);

        // convert the rest of the chars one by one 
        for (int i = first + 1; i < length; i++)
        {
            char ch = path[i];
            result.Append(_invalid.Contains(ch) ? replacement : ch);
        }

        string sanitizedPath = result.ToString();

        string toReturn = sanitizedPath;

        if (Path.DirectorySeparatorChar == '\\')
            toReturn = sanitizedPath.Replace('/', Path.DirectorySeparatorChar);
        else if (Path.DirectorySeparatorChar == '/')
            toReturn = sanitizedPath.Replace('\\', Path.DirectorySeparatorChar);

        return toReturn;
    }

    /// <summary>
    ///     Filesystem style widlcard match where * stands for any characters of any length and ? standa for one character
    /// </summary>
    /// <param name="s">input string</param>
    /// <param name="wildcard">wildcard</param>
    /// <returns>True if matches, false otherwise</returns>
    public static bool MatchesWildcard(this string s, string wildcard)
    {
        if (s == null) return false;
        if (wildcard == null) return false;

        wildcard = wildcard
            .Replace(".", "\\.") //escape '.' as it's a regex character
            .Replace("*", ".*")
            .Replace("?", ".");
        var rgx = new Regex(wildcard, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        return rgx.IsMatch(s);
    }

    #endregion

    #region Slugify

    public static string Slugify(this string phrase)
    {
        //var str = phrase.RemoveAccent().ToLower();
        var str = phrase.ToNonAccent().ToLower();

        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-");

        return str;
    }

    public static string RemoveAccent(this string txt)
    {
        var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        return Encoding.ASCII.GetString(bytes);
    }

    #endregion
}