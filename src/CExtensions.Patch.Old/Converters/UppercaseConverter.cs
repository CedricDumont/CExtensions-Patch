using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Mapper.Patch.Converters
{
    public class UppercaseConverter : ValueConverter
    {

        public override object Convert(object val)
        {
            if(val != null)
            {
                return val.ToString().ToUpper();
            }

            return val;
        }
    }
}
