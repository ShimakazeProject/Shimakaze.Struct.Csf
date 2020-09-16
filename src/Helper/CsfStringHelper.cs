using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shimakaze.Struct.Csf.Helper
{
    public static class CsfStringHelper
    {
        public static readonly byte[] STR_RAW = { 32, 82, 84, 83 };
        public static readonly byte[] WSTR_RAW = { 87, 82, 84, 83 };

        /// <summary>
        /// Serialize CSF data to .Net Object
        /// </summary>
        public static async Task SerializeAsync(this CsfString value, Stream stream)
        {
            // 字符串标记
            await stream.WriteAsync(value.IsWString ? WSTR_RAW : STR_RAW);
            // 字符串主要内容长度
            await stream.WriteAsync(BitConverter.GetBytes(value.Content.Length));
            // 字符串主要内容
            await stream.WriteAsync(Decoding(Encoding.Unicode.GetBytes(value.Content)));
            // 判断是否包含额外内容
            if (value.IsWString)// 存在额外内容1
            {
                var extra = value as CsfWString;
                // 额外内容长度
                await stream.WriteAsync(BitConverter.GetBytes(extra.Extra.Length));
                // 额外内容
                await stream.WriteAsync(Encoding.ASCII.GetBytes(extra.Extra));
            }
        }

        public static CsfString Create(string content) => new CsfString { IsWString = false, Content = content };
        public static CsfWString Create(string content, string extra) => new CsfWString { IsWString = true, Content = content, Extra = extra };
        /// <summary>
        /// Deserialize .Net Object to CSF data
        /// </summary>
        public static async Task<CsfString> DeserializeAsync(Stream stream, int bufferLength = 1024)
        {
            var buffer = new byte[bufferLength];
            await stream.ReadAsync(buffer, 0, 4);
            if (buffer.SequenceEqual(STR_RAW) || buffer.SequenceEqual(WSTR_RAW)) throw new FormatException();
            var isWStr = buffer.Take(4).SequenceEqual(WSTR_RAW);
            await stream.ReadAsync(buffer, 0, 4);
            var valueLength = BitConverter.ToInt32(buffer, 0);
            await stream.ReadAsync(buffer, 0, valueLength << 1);
            var value = Encoding.Unicode.GetString(Decoding(buffer, valueLength << 1), 0, valueLength << 1);

            // 判断是否包含额外内容
            if (isWStr)
            {
                await stream.ReadAsync(buffer, 0, 4);
                var extraLength = BitConverter.ToInt32(buffer, 0);
                await stream.ReadAsync(buffer, 0, extraLength);
                var extra = Encoding.ASCII.GetString(buffer, 0, extraLength);
                return Create(value, extra);
            }
            else
            {
                return Create(value);
            }
        }

        /// <summary>
        /// 值字符串 编/解码<br/>
        /// CSF文档中的Unicode编码内容都是按位异或的<br/>
        /// 这个方法使用for循环实现
        /// </summary>
        /// <param name="ValueData">内容</param>
        /// <returns>编/解码后的数组</returns>
        public static byte[] Decoding(byte[] ValueData)
        {
            for (int i = 0; i < ValueData.Length; ++i)
                ValueData[i] = (byte)~ValueData[i];
            return ValueData;
        }
        public static byte[] Decoding(byte[] ValueData, int ValueDataLength)
        {
            for (int i = 0; i < ValueDataLength; ++i)
                ValueData[i] = (byte)~ValueData[i];
            return ValueData;
        }
        public static CsfWString ToWString(this CsfString str, string extra = null)
        {
            if (str is CsfWString wstr) return wstr;
            else return new CsfWString()
            {
                IsWString = true,
                Content = str.Content,
                Extra = extra
            };
        }
        public static CsfString ToNormalString(this CsfString str)
        {
            if (str.GetType() == typeof(CsfString)) return str;
            else return Create(str.Content);
        }
    }
}
