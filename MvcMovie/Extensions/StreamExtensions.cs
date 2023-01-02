using System.Text;

namespace MvcMovie.Extensions;

public static class StreamExtensions
{
    public static byte[] ToByteArray(this Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        if (stream is MemoryStream memoryStream)
            return memoryStream.ToArray();

        using var streamReader = new MemoryStream();
        stream.CopyTo(streamReader);
        return streamReader.ToArray();
    }

    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        if (stream is MemoryStream memoryStream)
            return memoryStream.ToArray();

        using var streamReader = new MemoryStream();
        await stream.CopyToAsync(streamReader).ConfigureAwait(false);
        return streamReader.ToArray();
    }

    public static string AsString(this Stream stream) => stream.AsString(Encoding.UTF8);

    public static string AsString(this Stream stream, Encoding encoding)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        if (encoding == null) throw new ArgumentNullException(nameof(encoding));

        if (stream.CanSeek)
            stream.Position = 0;

        using var sr = new StreamReader(stream, encoding);
        string result = sr.ReadToEnd();

        return result;
    }

    public static Task<string> AsStringAsync(this Stream stream) => stream.AsStringAsync(Encoding.UTF8);

    public static Task<string> AsStringAsync(this Stream stream, Encoding encoding)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        if (encoding == null) throw new ArgumentNullException(nameof(encoding));

        Task<string> result;

        if (stream.CanSeek)
            stream.Position = 0;

        using (var sr = new StreamReader(stream, encoding))
        {
            result = sr.ReadToEndAsync();
        }

        return result;
    }
}