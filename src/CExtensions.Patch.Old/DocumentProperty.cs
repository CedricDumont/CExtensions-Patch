using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch
{
    public class DocumentProperty : ICloneable
    {
        public DocumentProperty(Path path, Object value, Boolean touched)
        {
            this.Path = path;
            this.Value = value;
            this.Touched = touched;
        }

        public Type Type
        {
            get
            {
                if (Value == null) { return null; }
                return Value.GetType();
            }
        }

        public Path Path { get; private set; }

        public Object Value { get; set; }

        public Boolean Touched { get; set; }

        public object Clone()
        {
            //Todo : don't think it's the good way (Path is a struct so will be copied)

            Object clonedValue = null;
            ICloneable clonableValue = Value as ICloneable;
            if (clonableValue != null)
            {
                clonedValue = clonableValue.Clone();
            }
            else
            {
                clonedValue = Value;
            }
           
            return new DocumentProperty(Path, clonedValue, this.Touched);
        }

        public override string ToString()
        {
            return "DocumentProperty : " + this.Path;
        }
    }
}
