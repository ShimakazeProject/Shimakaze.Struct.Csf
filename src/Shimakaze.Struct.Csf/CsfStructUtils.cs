using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shimakaze.Struct.Csf
{
    public static class CsfStructUtils
    {  /// <summary>
       /// Serialize CSF data to .Net Object
       /// </summary>
        public static CsfStruct Serialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
                return Serialize(br);
        }
        public static CsfStruct Serialize(BinaryReader binaryReader)
        {
            var doc = new CsfStruct
            {
                Head = CsfHeadUtils.Serialize(binaryReader),
            };
            doc.Data = new List<CsfLabel>(doc.Head.LabelCount);
            for (var i = 0; i < doc.Head.LabelCount; i++)
                doc.Data.Add(CsfLabelUtils.Serialize(binaryReader));
            return doc;
        }
        /// <summary>
        /// Deserialize .Net Object to CSF data
        /// </summary>
        public static void Deserialize(this ref CsfStruct @this, Stream stream)
        {
            using (var bw = new BinaryWriter(stream))
                @this.Deserialize(bw);
        }
        public static void Deserialize(this ref CsfStruct @this, BinaryWriter binaryWriter)
        {
            @this.Head.Deserialize(binaryWriter);
            @this.Data.ForEach(i => i.Deserialize(binaryWriter));
        }

        public static void ReCount(this ref CsfStruct @this)
        {
            @this.Head.LabelCount = @this.Data.Count;
            @this.Head.StringCount = @this.Data.Select(i => i.Values.Count).Sum();
        }
    }
}
