using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch.Operations
{
    public class AppendOperation : Operation
    {
        public AppendOperation(Path path)
            : base("Append", path)
        {
        }

        public Path ValueOf { get; set; }

        public override Boolean Execute(TargetDocument obj)
        {
            if (Value != null)
            {
                obj.CreateOrUpdateProperty(Path, obj[Path].ToString() + Value.ToString(), true);
                return true;
            }
            else
            {
                obj.CreateOrUpdateProperty(Path, obj[Path].ToString() + obj[ValueOf].ToString(), true);
                return true;
            }
        }

    }
}
