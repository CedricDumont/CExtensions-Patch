using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public class ReplaceOperation : Operation
    {

        public ReplaceOperation() : base("replace") { }
        protected override void DoExecute()
        {
            var existing_Token = Target.SelectToken(Path);

            if (existing_Token != null)
            {
                if (Value is JToken)
                {
                    existing_Token.Replace((JToken)Value);
                }
                else
                {
                    existing_Token.Replace(new JValue(Value));
                }
            }
        }
    }
}
