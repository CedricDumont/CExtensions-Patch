using CExtensions.Patch.Mapper.Patch.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;

namespace CExtensions.Patch.Mapper.Patch
{
    public abstract class Operation
    {
        public Operation(String name, Path path)
        {
            this.Name = name;
            this.Path = path;
        }

        public String Name { get; private set; }

        public Path Path { get; set; }

        public Object Value { get; set; }

        public ValueConverter Converter { get; set; }

        public Boolean Apply(TargetDocument obj)
        {
            InitValue(obj);

            if (Converter != null)
            {
                Value = Converter.Convert(Value);
            }

            Boolean result = Execute(obj);

            return result;
        }

        public abstract Boolean Execute(TargetDocument obj);

        protected virtual void InitValue(TargetDocument obj) { }

        public override string ToString()
        {
            return $"\"op\": \"{Name}\", \"path\": \"{Path}\", \"value\": {Value} ";
        }
    }
}
