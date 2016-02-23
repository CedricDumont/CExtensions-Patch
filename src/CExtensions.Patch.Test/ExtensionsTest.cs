using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CExtensions.Patch.Test
{
    public class ExtensionsTest : BaseUnitTest
    {
        //[Fact]
        //public void ShouldPatch()
        //{
        //    var originalDocument = @"
        //                         {
        //                            'Name': 'Cedric',
        //                         }";

        //    JsonPatchDocument doc = JsonPatchDocument.Create().Move("Name", "FirstName");

        //    originalDocument = originalDocument.Patch(doc);

        //    string result = TrimAllWhitespace(originalDocument);
        //    result.ShouldBe("{'FirstName':'Cedric'}");
        //}

        //[Fact]
        //public void ShouldTranformPersonToAnimal()
        //{
        //    Animal p = new Animal() { Name = "Snoopy" };

        //    JsonPatchDocument doc = JsonPatchDocument.Create().Move("Name", "FirstName");

        //    var patched = p.Patch(doc);

        //    string result = SerializeObjectAndTrimWhiteSpace(patched);
        //    result.ShouldBe("{'FirstName':'Snoopy'}");
        //}


      


    }

    public class Person
    {
        public string FirtsName { get; set; }
    }

    public class Animal
    {
        public string Name { get; set; }
    }


}
