using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public class CopyOperation : Operation
    {
        public CopyOperation() : base("copy") { }
        protected override void DoExecute()
        {
            var existingValue = Target.SelectToken(From);

            if (existingValue != null)
            {
                Operation operation = new AddOperation() { Target = Target, Path = Path, Value = existingValue };
                operation.Execute();
            }


        }
    }
}
