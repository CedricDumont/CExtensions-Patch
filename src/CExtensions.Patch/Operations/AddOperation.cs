using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public class AddOperation : Operation
    {
        public AddOperation() : base("add") { }

        protected override void DoExecute()
        {
            AddPath(Target, Path, Value);
        }

    }
}
