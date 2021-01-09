using System;
using System.IO;
using System.Linq;

namespace Shimakaze.Struct.Csf
{
    public static class CsfHeadUtils
    {
        public static CsfHead Serialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
                return Serialize(br);
        }
        public static CsfHead Serialize(BinaryReader binaryReader)
        {
            var result = new CsfHead();
            if (!binaryReader.ReadBytes(4).SequenceEqual(CsfHead.CSF_FLAG))
                throw new FormatException("Unknown File Format: Unknown Header");
            result.Version = binaryReader.ReadInt32();
            result.LabelCount = binaryReader.ReadInt32();
            result.StringCount = binaryReader.ReadInt32();
            result.Unknown = binaryReader.ReadInt32();
            result.LabelCount = binaryReader.ReadInt32();
            return result;
        }
        public static void Deserialize(this ref CsfHead @this, Stream stream)
        {
            using (var br = new BinaryWriter(stream))
                @this.Deserialize(br);
        }
        public static void Deserialize(this ref CsfHead @this, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(CsfHead.CSF_FLAG);
            binaryWriter.Write(@this.Version);
            binaryWriter.Write(@this.LabelCount);
            binaryWriter.Write(@this.StringCount);
            binaryWriter.Write(@this.Unknown);
            binaryWriter.Write(@this.Language);
        }
    }
}
