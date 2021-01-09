using System;
using System.IO;
using System.Text;


namespace Shimakaze.Struct.Csf
{
    public static class CsfValueUtils
    {
        public const int STR_RAW = 1398034976;
        public const int WSTR_RAW = 1398035031;

        /// <summary>
        /// Serialize CSF data to .Net Object
        /// </summary>
        public static CsfValue Serialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
                return Serialize(br);
        }
        public static CsfValue Serialize(BinaryReader binaryReader)
        {
            var result = new CsfValue();
            var flag = binaryReader.ReadInt32();
            // 判断是否包含额外内容
            switch (flag)
            {
                case STR_RAW:
                    ReadValue(ref result, binaryReader);
                    break;
                case WSTR_RAW:
                    ReadValue(ref result, binaryReader);
                    result.Extra = Encoding.ASCII.GetString(binaryReader.ReadBytes(binaryReader.ReadInt32()));
                    break;
                default:
                    throw new FormatException();
            }
            return result;
        }
        private static void ReadValue(ref CsfValue csfValue, BinaryReader binary)
        {
            var valueLength = binary.ReadInt32();
            var value = Encoding.Unicode.GetString(Decoding(binary.ReadBytes(valueLength << 1), valueLength << 1));
            csfValue.Value = value;
        }
        /// <summary>
        /// Deserialize .Net Object to CSF data
        /// </summary>
        public static void Deserialize(this ref CsfValue @this, Stream stream)
        {
            using (var bw = new BinaryWriter(stream))
                @this.Deserialize(bw);
        }
        public static void Deserialize(this ref CsfValue @this, BinaryWriter binaryWriter)
        {
            var flag = string.IsNullOrEmpty(@this.Extra);
            // 字符串标记
            binaryWriter.Write(flag ? STR_RAW : WSTR_RAW);
            // 字符串主要内容长度
            binaryWriter.Write(@this.Value.Length);
            // 字符串主要内容
            binaryWriter.Write(Decoding(Encoding.Unicode.GetBytes(@this.Value.Replace("\r\n", "\n"))));
            // 判断是否包含额外内容
            if (!flag)// 存在额外内容1
            {
                // 额外内容长度
                binaryWriter.Write(@this.Extra.Length);
                // 额外内容
                binaryWriter.Write(Encoding.ASCII.GetBytes(@this.Extra));
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
            for (var i = 0; i < ValueData.Length; ++i)
                ValueData[i] = (byte)~ValueData[i];
            return ValueData;
        }
        public static byte[] Decoding(byte[] ValueData, int ValueDataLength)
        {
            for (var i = 0; i < ValueDataLength; ++i)
                ValueData[i] = (byte)~ValueData[i];
            return ValueData;
        }
    }
}
