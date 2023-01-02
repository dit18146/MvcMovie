namespace MvcMovie.Extensions;

public static class TimespanExtensions
{
    public static CancellationToken ToCancellationToken(this TimeSpan timeout)
    {
        if (timeout == TimeSpan.Zero)
            return new CancellationToken(true);

        return timeout.Ticks > 0 ? new CancellationTokenSource(timeout).Token : default;
    }

    public static CancellationToken ToCancellationToken(this TimeSpan? timeout, TimeSpan defaultTimeout) =>
        (timeout ?? defaultTimeout).ToCancellationToken();

    public static TimeSpan Min(this TimeSpan source, TimeSpan other) => source.Ticks > other.Ticks ? other : source;

    public static TimeSpan Max(this TimeSpan source, TimeSpan other) => source.Ticks < other.Ticks ? other : source;
}