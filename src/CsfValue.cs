using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shimakaze.Struct.Csf
{
    public static class CsfValueHelper
    {
        /// <summary>
        /// 异步还原
        /// </summary>
        /// <param name="stream">流</param>
        public static async Task DeparseAsync(this CsfValue value, Stream stream)
        {
            // 字符串标记
            await stream.WriteAsync(value.IsWstr ? CsfValue.WSTR_RAW : CsfValue.STR_RAW);
            // 字符串主要内容长度
            await stream.WriteAsync(BitConverter.GetBytes(value.Content.Length));
            // 字符串主要内容
            await stream.WriteAsync(CsfValue.Decoding(Encoding.Unicode.GetBytes(value.Content)));
            // 判断是否包含额外内容
            if (value.IsWstr)// 存在额外内容
            {
                // 额外内容长度
                await stream.WriteAsync(BitConverter.GetBytes(value.Extra.Length));
                // 额外内容
                await stream.WriteAsync(Encoding.ASCII.GetBytes(value.Extra));
            }
        }
    }

    public class CsfValue
    {
        internal protected static readonly byte[] STR_RAW = { 32, 82, 84, 83 };
        internal protected static readonly byte[] WSTR_RAW = { 87, 82, 84, 83 };

        public string Content = string.Empty;
        public string Extra = string.Empty;
        public bool IsWstr = false;

        internal protected CsfValue() { }

        /// <summary>
        /// 值字符串 编/解码<br/>
        /// CSF文档中的Unicode编码内容都是按位异或的<br/>
        /// 这个方法使用for循环实现
        /// </summary>
        /// <param name="ValueData">内容</param>
        /// <returns>编/解码后的数组</returns>
        internal protected static byte[] Decoding(byte[] ValueData)
        {
            for (int i = 0; i < ValueData.Length; ++i)
                ValueData[i] = (byte)~ValueData[i];
            return ValueData;
        }

        public static CsfValue Create(string content) => new CsfValue { IsWstr = false, Content = content };
        public static CsfValue Create(string content, string extra) => new CsfValue { IsWstr = true, Content = content, Extra = extra };
        public static CsfValue CreateFromData(byte[] data) => new CsfValue { IsWstr = false, Content = Encoding.Unicode.GetString(Decoding(data)) };

        /// <summary>
        /// 异步解析
        /// </summary>
        public static async Task<CsfValue> ParseAsync(Stream stream)
        {
            var flag = await stream.ReadAsync(4);
            var content = CreateFromData(await stream.ReadAsync(BitConverter.ToInt32(await stream.ReadAsync(4), 0) << 1));
            // 判断是否包含额外内容
            if (flag.SequenceEqual(WSTR_RAW))// 存在额外内容
            {
                content.IsWstr = true;
                content.Extra = Encoding.ASCII.GetString(await stream.ReadAsync(BitConverter.ToInt32(await stream.ReadAsync(4), 0)));
            }
            return content;
        }
    }
}
