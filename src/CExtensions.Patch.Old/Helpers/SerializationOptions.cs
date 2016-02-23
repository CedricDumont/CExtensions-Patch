using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Utils.Serialization
{
    public class SerializationOptions
    {
        public static readonly SerializationOptions Default =
            new SerializationOptions();

        public static readonly SerializationOptions SurroundStringWithQuotes =
            new SerializationOptions() { SurroundPropertyNameWith = "\"", SurroundStringValueWith = "\"", onlyTouched = true };

        public SerializationOptions()
        {
            SurroundPropertyNameWith = "";
            SurroundStringValueWith = "";
               onlyTouched = false;

        }
        public String SurroundPropertyNameWith { get; set; }

        public String SurroundStringValueWith { get; set; }

        public Boolean onlyTouched { get; set; }


    }
}
