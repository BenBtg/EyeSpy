using System.IO;

namespace ExeSpy.Service.Common
{
    public static class StreamExtensions
    {
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] byteData;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byteData = ms.ToArray();
            }

            return byteData;
        }
    }
}