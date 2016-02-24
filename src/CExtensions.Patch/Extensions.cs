using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch
{
    public static class Extensions
    {
        public static JsonPatchDocument WithRemoveUntouchedProperties(this JsonPatchDocument jsonPatchDoc)
        {
            jsonPatchDoc.Options.RemoveUntouched = true;
            return jsonPatchDoc;
        }

        public static JsonPatchDocument WithErrorRecoveryDisabled(this JsonPatchDocument jsonPatchDoc)
        {
            jsonPatchDoc.Options.RecoverIfError = true;
            return jsonPatchDoc;
        }

        public static JsonPatchDocument Optimized(this JsonPatchDocument jsonPatchDoc)
        {
            jsonPatchDoc.Options.RecoverIfError = false;
            jsonPatchDoc.Options.RemoveUntouched = false;
            return jsonPatchDoc;
        }

    }
}
