using System.Text.Json;

namespace BolyukGame.Shared
{
    public class ByteUtils
    {
        public static byte[] Serialize<T>(T data)
        {
            return JsonSerializer.SerializeToUtf8Bytes<T>(data);
        }

        public static T Deserialize<T>(byte[] data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
