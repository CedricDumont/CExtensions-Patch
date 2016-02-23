using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Utils
{
    public class PathMetaData
    {
        public Boolean IsArray { get; set; }

        public Int32 ArrayIndex { get; set; }

        public Boolean IsObject { get; set; }

        public Boolean IsProperty
        {
            get
            {
                return !IsObject && !IsArray;
            }
        }

        public String Name { get; set; }

        public Boolean IsEmpty { get { return String.IsNullOrEmpty(this.Name); } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if(IsObject)
            {
                sb.Append("(Obj)");
            }
            if (IsProperty)
            {
                sb.Append("(Prop)");
            }

            sb.Append(Name);

            if(IsArray)
            {
                sb.Append("[");
                sb.Append(ArrayIndex);
                sb.Append("]");
            }

            return sb.ToString();

            
        }
    }
}
