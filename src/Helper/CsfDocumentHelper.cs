using System.IO;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Csf.Helper
{
    public static class CsfDocumentHelper
    {
        /// <summary>
        /// Serialize CSF data to .Net Object
        /// </summary>
        public static async Task SerializeAsync(this CsfDocument value, Stream stream)
        {
            await CsfHeadHelper.SerializeAsync(value.Head, stream);
            for (int i = 0; i < value.Count; i++)
                await CsfLabelHelper.SerializeAsync(value[i], stream);
        }
        /// <summary>
        /// Deserialize .Net Object to CSF data
        /// </summary>
        public static async Task<CsfDocument> DeserializeAsync(Stream stream)
        {
            var doc = new CsfDocument
            {
                Head = await CsfHeadHelper.DeserializeAsync(stream)
            };
            doc.Capacity = doc.Head.LabelCount;
            for (int i = 0; i < doc.Head.LabelCount; i++)
                doc.Add(await CsfLabelHelper.DeserializeAsync(stream));
            return doc;
        }
    }
}
