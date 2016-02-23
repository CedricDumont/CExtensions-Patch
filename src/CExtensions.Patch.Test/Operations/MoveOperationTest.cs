using CExtensions.Patch.Operations;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace CExtensions.Patch
{
    public class MoveOperationTest : BaseUnitTest
    {

        [Fact]
        public void ShouldMoveValueForSimpleProperty()
        {
            dynamic _Object = JObject.Parse(@"
                                {
                                 'baz': 'qux',
                                 'foo': 'bar'
                                 }"
                );

            Operation operation = new MoveOperation() { Target = _Object, From= "$.baz", Path = "$.boo"};
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'foo':'bar','boo':'qux'}");
        }

        [Fact]
        public void ShouldMoveValueForComplexProperty()
        {
            dynamic _Object = JObject.Parse(@"
                            {
                             'foo': {
                               'bar': 'baz',
                               'waldo': 'fred'
                             },
                             'qux': {
                               'corge': 'grault'
                             }
                           }"
                );

            Operation operation = new MoveOperation() { Target = _Object, From="$.foo.waldo", Path="$.qux.thud"};
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'foo':{'bar':'baz'},'qux':{'corge':'grault','thud':'fred'}}");
        }

        [Fact]
        public void ShouldMoveObjectBetweenTwoArray()
        {
            dynamic _Object = JObject.Parse(@"
                                {'baz': [{'name': 'foo'},{'name': 'bar'}],
                                 'boo': [{'name': 'zoo'},{'name': 'ear'}]}"
                );

            Operation operation = new MoveOperation() { Target = _Object, From = "$.baz[0]", Path = "$.boo[1]" };
            operation.Execute();
            string result = SerializeObject(_Object);
            result.ShouldBe(@"{'baz':[{'name':'bar'}],'boo':[{'name':'zoo'},{'name':'foo'},{'name':'ear'}]}");
        }

        //{ "foo": [ "all", "grass", "cows", "eat" ]}

       

    }
}
