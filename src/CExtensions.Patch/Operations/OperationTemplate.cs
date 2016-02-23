using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public class OperationTemplate
    {
        public String op { get; set; }

        public String from { get; set; }

        public String path { get; set; }

        public Object value { get; set; }

        public String AsJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"'op': '{op}'");
            if (!String.IsNullOrEmpty(from))
            {
                sb.Append($", 'from':'{from}'");
            }
            if (!String.IsNullOrEmpty(path))
            {
                sb.Append($",'path':'{path}'");
            }
            if (value != null)
            {
                string serializedValueForm = JsonConvert.SerializeObject(value);
                sb.Append($",'value':{serializedValueForm}");
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}
