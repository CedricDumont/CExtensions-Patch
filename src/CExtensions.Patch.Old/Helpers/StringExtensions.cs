using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CExtensions.Patch.Utils.Extensions
{
    public static class StringExtensions
    {

        public static IList<string> ToList(this String str, char separator)
        {
            if (str == null)
            {
                return null;
            }
            string[] array = str.Split(separator);
            return array.ToList();
        }

        public static IList<string> ToList(this String str, string separator)
        {
            if (str == null)
            {
                return null;
            }
            string[] array = str.Split(separator);
            return array.ToList();
        }

        public static string Flatten(this IList<string> list, char separator)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in list)
            {
                builder.Append(item);
                builder.Append(separator);
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }


        public static Boolean StartsWithOneOf(this string item, String[] list, Boolean ignoreCase = true)
        {
            foreach (var startString in list)
            {
                if (ignoreCase)
                {
                    if (item.ToLower().StartsWith(startString.ToLower()))
                    {
                        return true;
                    }
                }
                else if (item.StartsWith(startString))
                {
                    return true;
                }
            }
            return false;
        }

        public static string StripTags(this string content)
        {
            var cleaned = string.Empty;
            try
            {
                StringBuilder textOnly = new StringBuilder();
                using (var reader = XmlNodeReader.Create(new System.IO.StringReader("<xml>" + content + "</xml>")))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Text)
                            textOnly.Append(reader.ReadContentAsString());
                    }
                }
                cleaned = textOnly.ToString();
            }
            catch
            {
                //A tag is probably not closed. fallback to regex string clean.
                string textOnly = string.Empty;
                Regex tagRemove = new Regex(@"<[^>]*(>|$)");
                Regex compressSpaces = new Regex(@"[\s\r\n]+");
                textOnly = tagRemove.Replace(content, string.Empty);
                textOnly = compressSpaces.Replace(textOnly, " ");
                cleaned = textOnly;
            }

            return cleaned;
        }

        public static Stream AsStream(this string content)
        {
            if (content == null)
            {
                return null;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                MemoryStream destination = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                    writer.Flush();
                    stream.Position = 0;
                    stream.CopyTo(destination);
                }               
                destination.Position = 0;
                return destination;
            }

        }

        public static byte[] AsUTF8Bytes(this string str)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            return data;
        }

        public static void RemoveLast(this StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }

        public static String ValueIfNull(this String str, String valIfNull)
        {
            if (str == null)
            {
                return valIfNull;
            }
            return str;
        }

    }
}
