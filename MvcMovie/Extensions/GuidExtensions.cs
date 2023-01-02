using System.Security.Cryptography;
using System.Text;

namespace MvcMovie.Extensions;

public static class GuidExtension
{
    public static Guid SequentialGuid(this Guid guid)
    {
        DateTime now = DateTime.UtcNow;

        int days = now.Subtract(new DateTime(1900, 1, 1)).Days;

        int num = (int)(now.Subtract(DateTime.Today).TotalMilliseconds / 3.0);

        byte[] b = Guid.NewGuid().ToByteArray();

        byte[] bytes1 = BitConverter.GetBytes(days);

        for (int index = 0; index < 2; ++index)
            b[10 + index] = bytes1[1 - index];

        byte[] bytes2 = BitConverter.GetBytes(num);

        for (int index = 0; index < 4; ++index)
            b[12 + index] = bytes2[3 - index];

        return new Guid(b);
    }

    public static Guid DeterministicGuid(this Guid guid, string input)
    {
        var md5 = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));

        return new Guid(md5);
    }
}