using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shimakaze.Struct.Csf
{
    public static class CsfLabelUtils
    {
        public const int FLAG_RAW = 1279413280;

        /// <summary>
        /// Serialize CSF data to .Net Object
        /// </summary>
        public static void Deserialize(this ref CsfLabel @this, Stream stream)
        {
            using (var bw = new BinaryWriter(stream))
                @this.Deserialize(bw);
        }
        public static void Deserialize(this ref CsfLabel @this, BinaryWriter binaryWriter)
        {
            // 标签头
            binaryWriter.Write(FLAG_RAW);
            // 字符串数量 
            binaryWriter.Write(@this.Values.Count);
            // 标签名长度
            binaryWriter.Write(@this.Label.Length);
            // 标签名
            binaryWriter.Write(Encoding.ASCII.GetBytes(@this.Label));

            @this.Values.ForEach(i => i.Deserialize(binaryWriter));
        }
        /// <summary>
        /// Deserialize .Net Object to CSF data
        /// </summary>
        public static CsfLabel Serialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
                return Serialize(br);
        }
        public static CsfLabel Serialize(BinaryReader binaryReader)
        {
            if (binaryReader.ReadInt32() != FLAG_RAW)
                throw new NotSupportedException();
            var result = new CsfLabel();
            var count = binaryReader.ReadInt32();
            result.Values = new List<CsfValue>(count);
            result.Label = Encoding.ASCII.GetString(binaryReader.ReadBytes(binaryReader.ReadInt32()));
            for (var i = 0; i < count; i++)
                result.Values.Add(CsfValueUtils.Serialize(binaryReader));

            return result;
        }
    }
}
