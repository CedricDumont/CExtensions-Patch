using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch.Operations
{
    public class AddOperation : Operation
    {
        public AddOperation(Path path)
            : base("Add", path)
        {
        }

        public override Boolean Execute(TargetDocument obj)
        {
            obj.RemoveProperty(Path);

            obj.CreateOrUpdateProperty(Path,Value, true);

            return true;
        }
    }
}
