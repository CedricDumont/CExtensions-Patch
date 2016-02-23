using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Mapper.Patch
{
    public class PatchDocument
    {
        private List<Operation> _operations = new List<Operation>();

        public void AddOperation(Operation op)
        {
            this._operations.Add(op);
        }

        public IEnumerable<Operation> Operations
        {
            get
            {
                return _operations;
            }
        }
    }
}
