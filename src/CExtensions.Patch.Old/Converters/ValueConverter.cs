using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Mapper.Patch.Converters
{
    public abstract class ValueConverter
    {
        public abstract Object Convert(Object val);
    }
}
