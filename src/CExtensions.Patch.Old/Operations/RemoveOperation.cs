using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch.Operations
{
    public class RemoveOperation : Operation
    {
        public RemoveOperation(Path path)
            : base("Remove", path)
        {
        }


        public override Boolean Execute(TargetDocument obj)
        {
            //according to RFC 6902 if the path does not exists than the operation fails
            if (obj.ContainsPath(Path))
            {
                obj.RemoveProperty(Path);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
