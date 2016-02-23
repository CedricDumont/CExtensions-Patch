using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch.Operations
{
    public class MoveOperation : Operation
    {
        private String _from;

        public MoveOperation(String from)
            : base("Move", Path.Empty)
        {
            _from = from;

        }

        public override Boolean Execute(TargetDocument obj)
        {
            if (obj.ContainsPath(new Path(_from)))
            {
                //remove the property
                obj.RemoveProperty(new Path(_from));
                //add the new one
                obj.CreateOrUpdateProperty(Path, Value, true);
            }

            return true;
        }

        protected override void InitValue(TargetDocument obj)
        {
            if (obj.ContainsPath(new Path(_from)))
            {
                this.Value = obj[new Path(_from)];
            }
        }
       
    }
}
