using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shimakaze.Struct.Csf.Json;

namespace Shimakaze.Struct.Csf.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            await using var inCsf = new FileStream("InCsf.csf", FileMode.Open);
            await using var outJson = new FileStream("OutJson.json", FileMode.Create);
            await using var outCsf = new FileStream("OutCsf.csf", FileMode.Create);
            var csfStruct = CsfStructUtils.Serialize(inCsf);
            var options = CsfJsonConverterUtils.CsfJsonSerializerOptions;
            await JsonSerializer.SerializeAsync(outJson, csfStruct, options);
            outJson.Seek(0, SeekOrigin.Begin);
            var csfStruct1 = await JsonSerializer.DeserializeAsync<CsfStruct>(outJson, options);
            csfStruct1.Deserialize(outCsf);
        }
    }
}
