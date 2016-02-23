using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Mapper.Patch
{
    public class PatchException : Exception
    {
        public PatchException(string p) : base(p)
        {
        }
    }
}
