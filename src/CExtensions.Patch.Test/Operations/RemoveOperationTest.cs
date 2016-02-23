using CExtensions.Patch.Operations;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace CExtensions.Patch
{
    public class RemoveOperationTest : BaseUnitTest
    {

        [Fact]
        public void RemoveASimpleObjectMember_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                {
                                 'baz': 'qux',
                                 'foo': 'bar'
                                 }"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "baz"};
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'foo':'bar'}");
        }

        [Fact]
        public void RemoveAComplexObjectMember_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                {
                                 'baz': {'age': 1},
                                 'foo': 'bar'
                                 }"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "$.baz.age" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'foo':'bar'}");
        }

        [Fact]
        public void RemoveAValueFromAnArrayAtIndex0_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                {'baz': [{'name': 'foo'},{'name': 'bar'}]}"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "$.baz[0]" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':[{'name':'bar'}]}");
        }

        [Fact]
        public void RemoveAValueFromAnArrayAtIndex1_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                {'baz': [{'name': 'foo'},{'name': 'bar'},{'name': 'ooz'}]}"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "$.baz[1]" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':[{'name':'foo'},{'name':'ooz'}]}");
        }

        [Fact]
        public void RemoveAValueFromAValueArrayAtIndex1_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                {'baz': ['foo','bar','ooz']}"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "$.baz[1]" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':['foo','ooz']}");
        }

        [Fact]
        public void RemoveObjectIfEmpty_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                {
                                 'baz': {'age': 1},
                                 'foo': 'bar'
                                 }"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "$.baz.age" };
            operation.Execute();
            string result = SerializeObjectAndTrimWhiteSpace(_Object);
            result.ShouldBe(@"{'foo':'bar'}");
        }

        [Fact]
        public void RemoveArrayIfEmpty_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                {'baz': ['foo']}"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "$.baz[0]" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{}");
        }

        [Fact]
        public void RemoveArrayWithMultipleEntryIfOneEntryEmpty_ShouldRemoveMember()
        {
            dynamic _Object = JObject.Parse(@"
                                { 'baz' : [{'prop1':'1','prop2':'2'},{'prop1':'1','prop2':'2'}]}"
                );

            Operation operation = new RemoveOperation() { Target = _Object, Path = "$.baz[0].prop1" };
            operation.Execute();
            operation = new RemoveOperation() { Target = _Object, Path = "$.baz[0].prop2" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':[{'prop1':'1','prop2':'2'}]}");

            //second part remove the array if empty
            operation = new RemoveOperation() { Target = _Object, Path = "$.baz[0].prop1" };
            operation.Execute();
            operation = new RemoveOperation() { Target = _Object, Path = "$.baz[0].prop2" };
            operation.Execute();
            result = SerializeObject(_Object);
            result.ShouldBe(@"{}");
        }

        
    }
}
