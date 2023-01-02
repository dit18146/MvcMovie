namespace MvcMovie.Extensions;

public static class NumericExtensions
{
    public static string ToFileSizeDisplay(this int i) => ToFileSizeDisplay((long)i, 2);

    public static string ToFileSizeDisplay(this int i, int decimals) => ToFileSizeDisplay((long)i, decimals);

    public static string ToFileSizeDisplay(this long i) => ToFileSizeDisplay(i, 2);

    public static string ToFileSizeDisplay(this long i, int decimals)
    {
        if (i < 1024 * 1024 * 1024) // 1 GB
        {
            string value = Math.Round(i / 1024m / 1024m, decimals).ToString("N" + decimals);

            if (decimals > 0 && value.EndsWith(new string('0', decimals)))
                value = value.Substring(0, value.Length - decimals - 1);

            return string.Concat(value, " MB");
        }
        else
        {
            string value = Math.Round(i / 1024m / 1024m / 1024m, decimals).ToString("N" + decimals);

            if (decimals > 0 && value.EndsWith(new string('0', decimals)))
                value = value.Substring(0, value.Length - decimals - 1);

            return string.Concat(value, " GB");
        }
    }

    public static string ToOrdinal(this int num)
    {
        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num.ToString("#,###0") + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num.ToString("#,###0") + "st";
            case 2:
                return num.ToString("#,###0") + "nd";
            case 3:
                return num.ToString("#,###0") + "rd";
            default:
                return num.ToString("#,###0") + "th";
        }
    }
}