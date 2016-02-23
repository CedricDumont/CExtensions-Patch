using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public class MoveOperation : Operation
    {
        public MoveOperation() : base("move") { }

        protected override void DoExecute()
        {
            var existingValue = Target.SelectToken(From);

            if (existingValue != null)
            {
                Operation removeoperation = new RemoveOperation() { Target = Target, Path = From};
                removeoperation.Execute();
                Operation addOperation = new AddOperation() { Target = Target, Path = Path, Value = existingValue };
                addOperation.Execute();
            }
        }
    }
}
