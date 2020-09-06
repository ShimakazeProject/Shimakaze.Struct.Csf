using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Csf.Helper
{
    internal static class StreamExtension
    {
        public static async Task<byte[]> ReadAsync(this Stream stream, int length)
        {
            var buffer = new byte[length];
            await stream.ReadAsync(buffer, 0, length);
            return buffer;
        }

        public static Task WriteAsync(this Stream stream, byte[] data) => stream.WriteAsync(data, 0, data.Length);
    }
}
