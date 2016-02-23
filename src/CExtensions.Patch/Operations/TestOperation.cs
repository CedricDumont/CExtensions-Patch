using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public class TestOperation : Operation
    {
        public TestOperation() : base("test") { }

        protected override void DoExecute()
        {
            IsInError = true; 

            JToken valueAsJValue = new JValue(Value);

            var existing_Token = Target.SelectToken(Path);

            if (existing_Token != null)
            {
                if(existing_Token.Equals(valueAsJValue))
                {
                    IsInError = false;
                }
            }
        }
    }
}
