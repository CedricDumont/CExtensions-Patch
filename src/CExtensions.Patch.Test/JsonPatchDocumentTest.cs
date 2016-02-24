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
        public async Task ShouldApplyPatch_2()
        {
            var originalDocument = JObject.Parse(@"
                                {
                                    'bar': 'qux',
                                    'baz': {'prop1':'1','prop2':'2'},
                                    'foo' : [{'prop1':'1','prop2':'2'},{'prop1':'3','prop2':'4'}]
                                 }");

            string jsonDocument = (@"
                                [
                                    { 'op': 'move', 'from': '$.foo', 'path': 'zoo' },
                                    { 'op': 'remove', 'path': '$.zoo[0]' },
                                    { 'op': 'move', 'from':'$.bar','path': '$.bal' },
                                    { 'op': 'add', 'path': '$.loo[0].newprop', 'value': 'loolipop' },
                                    { 'op': 'add', 'path': '$.tic.tac', 'value': '100ms' },
                                    { 'op': 'remove', 'path': '$.baz.prop1' },
                                ]"
                                );
            var doc = JsonPatchDocument.FromJson(jsonDocument).Optimized();

            await doc.ApplyTo(originalDocument);

            string result = SerializeObject(originalDocument);
            result.ShouldBe("{'baz':{'prop2':'2'},'zoo':[{'prop1':'3','prop2':'4'}],'bal':'qux','loo':[{'newprop':'loolipop'}],'tic':{'tac':'100ms'}}");

        }

        [Fact]
        public async Task ShouldApplyPatch_andConvertToArray()
        {
            var originalDocument = TrimAllWhitespace(@"
                                {
                                    'firstProp':'firstValue',
                                    'secondProp':'secondValue',
                                 }");

            string patch = (@"
                                [
                                    { 'op': 'move', 'from': 'firstProp', 'path': 'address[0].sameProp' },
                                    { 'op': 'move', 'from': 'secondProp', 'path': 'address[1].sameProp' },
                                ]"
                                );

            string expected = TrimAllWhitespace(@"{  
                                       'address':[
                                          {  
                                             'sameProp':'firstValue'
                                          },
                                          {  
                                             'sameProp':'secondValue'
                                          }
                                       ]
                                    }");


            var actual = await JsonPatchDocument.FromJson(patch).Optimized().ApplyTo(originalDocument);

            TrimAllWhitespace(actual).ShouldBe(expected);
        }

        [Fact]
        public async Task ShouldApplyPatch_ConcreteSample()
        {
            var originalDocument = JObject.Parse(@"
                                {
                                    'name':{'last_Name': 'Dumont', 'first_Name': 'Cedric'},
                                    'personal_adress_street':'heavenstreet',
                                    'personal_adress_nr':'10',
                                    'personal_adress_postCode':'4444',
                                    'personal_adress_City':'Paradise',
                                    'office_adress_street':'badStreet',
                                    'office_adress_nr':'666',
                                    'office_adress_postCode':'5555',
                                    'office_adress_City':'Hell',
                                 }");

            string jsonDocument = (@"
                                [
                                    { 'op': 'move', 'from': 'name.last_Name', 'path': 'familyName' },
                                    { 'op': 'move', 'from': 'name.first_Name', 'path': 'givenName' },
                                    { 'op': 'move', 'from': 'personal_adress_street', 'path': 'address[0].streetAddress' },
                                    { 'op': 'move', 'from': 'personal_adress_nr', 'path': 'address[0].postOfficeBoxNumber' },
                                    { 'op': 'move', 'from': 'personal_adress_postCode', 'path': 'address[0].postalCode' },
                                    { 'op': 'move', 'from': 'personal_adress_City', 'path': 'address[0].addressLocality' },
                                    { 'op': 'add', 'path': 'address[0].contactType','value':'private' },
                                    { 'op': 'move', 'from': 'office_adress_street', 'path': 'address[1].streetAddress' },
                                    { 'op': 'move', 'from': 'office_adress_nr', 'path': 'address[1].postOfficeBoxNumber' },
                                    { 'op': 'move', 'from': 'office_adress_postCode', 'path': 'address[1].postalCode' },
                                    { 'op': 'move', 'from': 'office_adress_City', 'path': 'address[1].addressLocality' },
                                    { 'op': 'add', 'path': 'address[1].contactType','value':'office' }                                   
                                ]"
                                );

            var doc = JsonPatchDocument.FromJson(jsonDocument).Optimized();

            await doc.ApplyTo(originalDocument);

            string actual = SerializeObject(originalDocument);
            string expected = SerializeObjectAndTrimWhiteSpace(originalDocument);

            actual.ShouldBe(expected);

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
                                    'arr' : [{'prop1':'1','prop2':'2'},{'prop1':'3','prop2':'4'}]
                                 }");

            var doc = JsonPatchDocument.Create().WithRemoveUntouchedProperties().Move("obj.prop1", "objnew");

            await doc.ApplyTo(originalDocument);

            string result = SerializeObject(originalDocument);
            result.ShouldBe("{'objnew':'1'}");
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
