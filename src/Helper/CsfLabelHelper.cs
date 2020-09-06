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
        public static async Task<CsfLabel> DeserializeAsync(Stream stream)
        {
            if (!(await stream.ReadAsync(4)).SequenceEqual(FLAG_RAW)) throw new FormatException("Unknown File Format");
            var lbl = new CsfLabel { Name = Encoding.ASCII.GetString(await stream.ReadAsync(BitConverter.ToInt32(await stream.ReadAsync(4), 0))) };
            lbl.Capacity = BitConverter.ToInt32(await stream.ReadAsync(4), 0);
            for (int i = 0; i < lbl.Capacity; i++) lbl.Add(await CsfStringHelper.DeserializeAsync(stream));
            return lbl;
        }
    }
}
