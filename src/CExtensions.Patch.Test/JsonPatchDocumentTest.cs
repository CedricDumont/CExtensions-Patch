using CExtensions.Patch.Operations;
using Marvin.JsonPatch;
using Newtonsoft.Json.Linq;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace CExtensions.Patch
{
    public class JsonPatchDocumentTest : BaseUnitTest
    {

        [Fact]
        public void ShouldCreateOperations()
        {
            string jsonDocument = (@"
                                [
                                    { 'op': 'add', 'path': '$.baz', 'value': 'qux' },
                                    { 'op': 'remove', 'path': '$.baz' },
                                    { 'op': 'test', 'path': '$.baz', 'value': {'property':1 } },
                                    { 'op': 'replace', 'path': '$.baz','value': 1},
                                    { 'op': 'copy', 'from':'$.bar','path': '$.baz' },
                                    { 'op': 'move', 'from':'$.bar','path': '$.baz' }
                                ]"
                                 );

            var doc = JsonPatchDocument.FromJson(jsonDocument);

            doc.Count.ShouldBe(6);
            doc[0].Name.ShouldBe("add");
            doc[0].Path.ShouldBe("$.baz");
            doc[0].Value.ShouldBe("qux");
            doc[1].Name.ShouldBe("remove");
            JObject valueof2 = JObject.Parse(@"{'property':1 }");
            doc[2].Value.ShouldBe(valueof2);
            doc[3].Value.ShouldBe(1);
            doc[4].From.ShouldBe("$.bar");
            doc[5].From.ShouldBe("$.bar");

        }

        [Fact]
        public async Task ShouldApplyPatch()
        {
            dynamic originalDocument = JObject.Parse(@"
                                {
                                 'baz': 'qux',
                                 'foo': 'bar'
                                 }"
               );

            string jsonDocument = (@"
                                [
                                    { 'op': 'add', 'path': '$.boo', 'value': 'zoo' },
                                    { 'op': 'remove', 'path': '$.baz' },
                                    { 'op': 'copy', 'from':'$.foo','path': '$.qux' }
                                ]"
                                );
            var doc = JsonPatchDocument.FromJson(jsonDocument);

            await doc.ApplyTo(originalDocument);

            string result = SerializeObject(originalDocument);
            result.ShouldBe("{'foo':'bar','boo':'zoo','qux':'bar'}");

        }

        [Fact]
        public async Task ShouldBuildPatch()
        {
            dynamic originalDocument = JObject.Parse(@"
                                {
                                 'baz': 'qux',
                                 'foo': 'bar'
                                 }"
               );

            var doc = JsonPatchDocument
                            .Create()
                            .Add("$.boo", "zoo")
                            .Remove("$.baz")
                            .Copy("$.foo", "$.qux");

            await doc.ApplyTo(originalDocument);

            string result = SerializeObject(originalDocument);
            result.ShouldBe("{'foo':'bar','boo':'zoo','qux':'bar'}");

        }

        [Fact]
        public async Task ShouldKeepTouchedOnly()
        {
            var originalDocument = JObject.Parse(@"
                                {
                                    'prop': 'qux',
                                    'obj': {'prop1':'1','prop2':'2'},
                                    'arr' : [{'prop1':'1','prop2':'2'},{'prop1':'1','prop2':'2'}]
                                 }");

            var doc = JsonPatchDocument.Create().Move("obj.prop1", "objnew");

            await doc.ApplyTo(originalDocument, true);

            string result = SerializeObject(originalDocument);
            result.ShouldBe("{'objnew':'1'");







        }

        [Fact]
        public void ShouldSerializeDocument()
        {
            var doc = JsonPatchDocument
                            .Create()
                            .Add("$.boo", "zoo")
                            .Remove("$.baz")
                            .Copy("$.foo", "$.qux");

            string asJson = TrimAllWhitespace(@"[
                                    { 'op': 'add', 'path': '$.boo', 'value': 'zoo' },
                                    { 'op': 'remove', 'path': '$.baz' },
                                    { 'op': 'copy', 'from':'$.foo','path': '$.qux' }
                                ]"
            );

            TrimAllWhitespace(doc.Serialize()).ShouldBe(asJson);
        }
    }
}
