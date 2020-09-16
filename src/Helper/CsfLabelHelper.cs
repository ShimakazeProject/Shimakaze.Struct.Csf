using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shimakaze.Struct.Csf.Helper
{
    public static class CsfLabelHelper
    {
        public static readonly byte[] FLAG_RAW = { 32, 76, 66, 76 };

        /// <summary>
        /// Serialize CSF data to .Net Object
        /// </summary>
        public static async Task SerializeAsync(this CsfLabel label, Stream stream)
        {
            // 标签头
            await stream.WriteAsync(FLAG_RAW);
            // 字符串数量 
            await stream.WriteAsync(BitConverter.GetBytes(label.Count));
            // 标签名长度
            await stream.WriteAsync(BitConverter.GetBytes(label.Name.Length));
            // 标签名
            await stream.WriteAsync(Encoding.ASCII.GetBytes(label.Name));

            foreach (var item in label) await item.SerializeAsync(stream);
        }
        /// <summary>
        /// Deserialize .Net Object to CSF data
        /// </summary>
        public static async Task<CsfLabel> DeserializeAsync(Stream stream, int bufferLength = 1024)
        {
            var buffer = new byte[bufferLength];
            await stream.ReadAsync(buffer, 0, 4);
            if (!buffer.Take(4).SequenceEqual(FLAG_RAW))
            {
                var sb = new StringBuilder("Unknown File Format, Flag is : [", 50);
                for (int i = 0; i < 4; i++)
                    sb.Append($"{buffer[i]:X}, ");
                sb.Append("].");
                throw new FormatException(sb.ToString());
            }
            var lbl = new CsfLabel();
            await stream.ReadAsync(buffer, 0, 4);
            lbl.Capacity = BitConverter.ToInt32(buffer, 0);
            await stream.ReadAsync(buffer, 0, 4);
            var nameLength = BitConverter.ToInt32(buffer, 0);
            await stream.ReadAsync(buffer, 0, nameLength);
            lbl.Name = Encoding.ASCII.GetString(buffer, 0, nameLength);
            for (int i = 0; i < lbl.Capacity; i++) lbl.Add(await CsfStringHelper.DeserializeAsync(stream));
            return lbl;
        }
    }
}
