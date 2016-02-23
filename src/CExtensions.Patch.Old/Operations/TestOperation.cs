using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch.Operations
{
    public class TestOperation : Operation
    {

        public TestOperation(Path path)
            : base("Test", path)
        {
        }

        public Object ExpectedResult
        {
            get;
            set;
        }

        public override Boolean Execute(TargetDocument obj)
        {
            if (obj.ContainsPath(Path))
            {
                Object valueToTest = obj[Path];

                return valueToTest.Equals(ExpectedResult);
            }

            return false;
        }
    }
}
