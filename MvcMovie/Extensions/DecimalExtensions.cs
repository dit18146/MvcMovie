namespace MvcMovie.Extensions;

public static class DecimalExtensions
{
    /// <summary>
    ///     The numbers percentage
    /// </summary>
    /// <param name="number">The number.</param>
    /// <param name="percent">The percent.</param>
    /// <returns>The result</returns>
    public static decimal PercentageOf(this decimal number, int percent) => number * percent / 100;


    /// <summary>
    ///     The numbers percentage
    /// </summary>
    /// <param name="number">The number.</param>
    /// <param name="percent">The percent.</param>
    /// <returns>The result</returns>
    public static decimal PercentageOf(this decimal number, decimal percent) => number * percent / 100;


    /// <summary>
    ///     The numbers percentage
    /// </summary>
    /// <param name="number">The number.</param>
    /// <param name="percent">The percent.</param>
    /// <returns>The result</returns>
    public static decimal PercentageOf(this decimal number, long percent) => number * percent / 100;

    /// <summary>
    ///     Percentage of the number.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="total"></param>
    /// <returns>The result</returns>
    public static decimal PercentOf(this decimal position, long total)
    {
        decimal result = 0;
        if (position > 0 && total > 0)
            result = position / total * 100;
        return result;
    }

    /// <summary>
    ///     Percentage of the number.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="total"></param>
    /// <returns>The result</returns>
    public static decimal PercentOf(this decimal position, int total)
    {
        decimal result = 0;
        if (position > 0 && total > 0)
            result = position / total * 100;
        return result;
    }

    /// <summary>
    ///     Percentage of the number.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="total"></param>
    /// <returns>The result</returns>
    public static decimal PercentOf(this decimal position, decimal total)
    {
        decimal result = 0;
        if (position > 0 && total > 0)
            result = position / total * 100;
        return result;
    }
}