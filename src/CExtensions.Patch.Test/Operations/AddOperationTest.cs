using CExtensions.Patch.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CExtensions.Patch
{
    public class AddOperationTest : BaseUnitTest
    {
        public dynamic _emptyObject = JObject.Parse("{}");

        [Fact]
        public void AddOperationOnEmptyObject_ShouldAddSimpleProperty()
        {
            Operation operation = new AddOperation() { Target = _emptyObject, Path = "FirstName", Value = "Cedric" };
            operation.Execute();
            ((string)_emptyObject.FirstName).ShouldBe("Cedric");
        }

        [Fact]
        public void AddOperationOnEmptyObject_ShouldAddComplexProperty()
        {
            Operation operation = new AddOperation() { Target = _emptyObject, Path = "Name.FirstName", Value = "Cedric" };
            operation.Execute();
            ((string)_emptyObject.Name.FirstName).ShouldBe("Cedric");
        }

        [Fact]
        public void AddOperationOnEmptyObject_ShouldAddArray()
        {
            Operation operation = new AddOperation() { Target = _emptyObject, Path = "Address[0].Street", Value = "Paradise Lane" };
            operation.Execute();
            ((string)_emptyObject.Address[0].Street).ShouldBe("Paradise Lane");
        }

        [Fact]
        public void AddOperationOnExistingProperties_Should_ReplaceValueForSimpleProperty()
        {
            _emptyObject.FirstName = "ToBeReplaced";
            Operation operation = new AddOperation() { Target = _emptyObject, Path = "FirstName", Value = "Cedric" };
            operation.Execute();
            ((string)_emptyObject.FirstName).ShouldBe("Cedric");
        }

        [Fact]
        public void AddOperationOnExistingProperties_ShouldReplaceValueForComplexProperty()
        {
            _emptyObject.Name = JObject.Parse("{}");
            _emptyObject.Name.FirstName = "ToBeReplaced";
            Operation operation = new AddOperation() { Target = _emptyObject, Path = "Name.FirstName", Value = "Cedric" };
            operation.Execute();
            ((string)_emptyObject.Name.FirstName).ShouldBe("Cedric");
        }

        [Fact]
        public void AddOperationOnExistingProperties_ShouldReplaceValueForArrayProperty()
        {
            _emptyObject.Address = JArray.Parse("[{'Street':'Hell Lane'}]");
            Operation operation = new AddOperation() { Target = _emptyObject, Path = "Address[0].Street", Value = "Paradise Lane" };
            operation.Execute();
            ((string)_emptyObject.Address[0].Street).ShouldBe("Paradise Lane");
        }

        [Fact]
        public void ShouldInsertValueAtIndexContainedInArray()
        {
            dynamic obj = JObject.Parse(@"{'baz': [{'name': 'foo'},{'name': 'bar'}]}");
            dynamic objToBeInserted = JObject.Parse(@"{'name': 'boo'}");

            Operation operation = new AddOperation() { Target = obj, Path = "$.baz[1]", Value=objToBeInserted };
            operation.Execute();
            string result = SerializeObject(obj);
            result.ShouldBe(@"{'baz':[{'name':'foo'},{'name':'boo'},{'name':'bar'}]}");
        }

        [Fact]
        public void ShouldAddPropertyAtObjectInIndexContainedInArray()
        {
            dynamic obj = JObject.Parse(@"{'baz': [{'name': 'foo'},{'name': 'bar'}]}");

            Operation operation = new AddOperation() { Target = obj, Path = "$.baz[1].lastName", Value = "myLastName" };
            operation.Execute();
            string result = SerializeObject(obj);
            result.ShouldBe(@"{'baz':[{'name':'foo'},{'name':'bar','lastName':'myLastName'}]}");
        }

        [Fact]
        public void ShouldInsertValueAtIndexNotContainedInArray()
        {
            dynamic obj = JObject.Parse(@"{'baz': [{'name': 'foo'},{'name': 'bar'}]}");
            dynamic objToBeInserted = JObject.Parse(@"{'name': 'boo'}");

            Operation operation = new AddOperation() { Target = obj, Path = "$.baz[2]", Value = objToBeInserted };
            operation.Execute();
            string result = SerializeObject(obj);
            result.ShouldBe(@"{'baz':[{'name':'foo'},{'name':'bar'},{'name':'boo'}]}");
        }

    }

}
