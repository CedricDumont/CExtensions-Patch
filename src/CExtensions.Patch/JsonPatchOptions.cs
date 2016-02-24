using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch
{
    public class JsonPatchOptions
    {
        public static JsonPatchOptions Default = new JsonPatchOptions();

        public JsonPatchOptions()
        {
            RemoveUntouched = false;
            RecoverIfError = false;
        }

        public Boolean RemoveUntouched
        {
            get; set;
        }

        public Boolean RecoverIfError
        {
            get; set;
        }

        public Boolean _traceOriginalDocumentSet = false;
        public Boolean _traceOriginalDocument = false;

        public Boolean TraceOriginalDocument
        {
            get
            {
                if (RecoverIfError || RemoveUntouched)
                {
                    return true;
                }

                if (_traceOriginalDocumentSet)
                {
                    return _traceOriginalDocument;
                }
               
                return false;
            }
            set
            {
                _traceOriginalDocumentSet = true;
                _traceOriginalDocument = value;
            }
        }
    }
}
