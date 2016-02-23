using CExtensions.Patch.Operations;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace CExtensions.Patch
{
    public class TestOperationTest : BaseUnitTest
    {

        [Fact]
        public void ShouldResultInSuccessfullTest()
        {
            dynamic _Object = JObject.Parse(@"
                               {
                                 'baz': 'qux',
                                 'foo': [ 'a', 2, 'c'],
                                 'bar': {'littleBar': '123456789'}
                                }"
                );

            Operation operation = new TestOperation() { Target = _Object, Path = "$.baz", Value="qux" };
            operation.Execute();
            operation.IsInError.ShouldBe(false);
            operation = new TestOperation() { Target = _Object, Path = "$.foo[1]", Value = 2 };
            operation.Execute();
            operation.IsInError.ShouldBe(false);
            operation = new TestOperation() { Target = _Object, Path = "$.bar.littleBar", Value = "123456789" };
            operation.Execute();
            operation.IsInError.ShouldBe(false);
        }

        [Fact]
        public void ShouldResultInUnSuccessfullTest()
        {
            dynamic _Object = JObject.Parse(@"
                               {
                                 'baz': 'qux',
                                 'foo': [ 'a', 2, 'c'],
                                 'bar': {'littleBar': '123456789'}
                                }"
                );

            Operation operation = new TestOperation() { Target = _Object, Path = "$.baz", Value = "xuq" };
            operation.Execute();
            operation.IsInError.ShouldBe(true);
            operation = new TestOperation() { Target = _Object, Path = "$.foo[1]", Value = "2" };
            operation.Execute();
            operation.IsInError.ShouldBe(true);
            operation = new TestOperation() { Target = _Object, Path = "$.bar.littleBar", Value = 123456789 };
            operation.Execute();
            operation.IsInError.ShouldBe(true);
        }
    }
}
