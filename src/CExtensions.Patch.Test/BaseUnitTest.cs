using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CExtensions.Patch
{
    public class BaseUnitTest
    {

        public String SerializeObjectAndTrimWhiteSpace(Object obj)
        {
            return TrimAllWhitespace(SerializeObject(obj));
        }

        public String SerializeObject(Object obj)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';

                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(writer, obj);
            }

            return sb.ToString();
        }

        public String TrimAllWhitespace(string str)
        {
            var result =  Regex.Replace(str, @"\s+", "");
            result = result.Replace('"', '\'');
            return result;

        }
    }
}
