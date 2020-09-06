using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shimakaze.Struct.Csf
{
    public static class CsfLabelHelper
    {
        /// <summary>
        /// 异步还原
        /// </summary>
        public static async Task DeparseAsync(this CsfLabel label, Stream stream)
        {
            // 标签头
            await stream.WriteAsync(CsfLabel.FLAG_RAW);
            // 字符串数量 
            await stream.WriteAsync(BitConverter.GetBytes(label.Count));
            // 标签名长度
            await stream.WriteAsync(BitConverter.GetBytes(label.Name.Length));
            // 标签名
            await stream.WriteAsync(Encoding.ASCII.GetBytes(label.Name));

            foreach (var item in label) await item.DeparseAsync(stream);
        }
    }

    /// <summary>
    /// CSF文件的标签结构
    /// </summary>
    public class CsfLabel : List<CsfValue>
    {
        internal protected static readonly byte[] FLAG_RAW = { 32, 76, 66, 76 };
        public string Name = string.Empty;
        internal protected CsfLabel() { }
        public static CsfLabel Create(string name) => new CsfLabel { Name = name };

        /// <summary>
        /// 异步解析
        /// </summary>
        public static async Task<CsfLabel> ParseAsync(Stream stream)
        {
            if (!(await stream.ReadAsync(4)).SequenceEqual(FLAG_RAW)) throw new FormatException("Unknown File Format");
            var lbl = Create(Encoding.ASCII.GetString(await stream.ReadAsync(BitConverter.ToInt32(await stream.ReadAsync(4), 0))));
            for (int i = 0; i < BitConverter.ToInt32(await stream.ReadAsync(4), 0); i++) lbl.Add(await CsfValue.ParseAsync(stream));
            return lbl;
        }
    }
}
