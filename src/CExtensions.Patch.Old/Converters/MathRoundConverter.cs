using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Mapper.Patch.Converters
{
    public class MathRoundConverter : ValueConverter
    {
        private int _numOfDecimals;
        public MathRoundConverter(int numOfDecimals){
            _numOfDecimals = numOfDecimals;
        }

        public override object Convert(object val)
        {
            double num = 0d;

            Boolean isDouble = Double.TryParse(val.ToString(), out num);

            if(isDouble)
            {
                val = Math.Round(num, _numOfDecimals);
            }

            return val;
        }
    }
}
