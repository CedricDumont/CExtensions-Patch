using CExtensions.Patch.Operations;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace CExtensions.Patch
{
    public class ReplaceOperationTest : BaseUnitTest
    {

        [Fact]
        public void ShouldReplaceValueForSimpleProperty()
        {
            dynamic _Object = JObject.Parse(@"
                                {
                                 'baz': 'qux',
                                 'foo': 'bar'
                                 }"
                );

            Operation operation = new ReplaceOperation() { Target = _Object, Path = "$.baz", Value="boo"};
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':'boo','foo':'bar'}");
        }

        [Fact]
        public void ShouldReplaceValueForComplexProperty()
        {
            dynamic _Object = JObject.Parse(@"
                                {
                                 'baz': {'age': 1},
                                 'foo': 'bar'
                                 }"
                );

            Operation operation = new ReplaceOperation() { Target = _Object, Path = "$.baz.age", Value=2};
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':{'age':2},'foo':'bar'}");
        }

        [Fact]
        public void ShouldReplaceValueForArray()
        {
            dynamic _Object = JObject.Parse(@"
                                {'baz': [{'name': 'foo'},{'name': 'bar'}]}"
                );

            Operation operation = new ReplaceOperation() { Target = _Object, Path = "$.baz[0].name", Value="boo" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':[{'name':'boo'},{'name':'bar'}]}");
        }

        [Fact]
        public void ShouldReplaceObjectForArray()
        {
            dynamic _Object = JObject.Parse(@"
                                {'baz': [{'name': 'foo'},{'name': 'bar'}]}"
                );
            dynamic _replacer = JObject.Parse(@"
                                {'name': 'boo'}"
                );

            Operation operation = new ReplaceOperation() { Target = _Object, Path = "$.baz[1]", Value = _replacer };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':[{'name':'foo'},{'name':'boo'}]}");
        }

    }
}
