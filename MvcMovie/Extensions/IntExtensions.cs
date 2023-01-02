namespace MvcMovie.Extensions;

public static class IntExtensions
{
    public static char ToHex(this int value)
    {
        if (value <= 9)
            return (char)(value + 48);

        return (char)(value - 10 + 97);
    }
}