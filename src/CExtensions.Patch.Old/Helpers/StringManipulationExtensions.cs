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
    public enum StringManipulationMode
    {
        Inclusive,
        Exclusive
    }

    public static class StringManipulationExtensions
    {
       
        /// <summary>
        /// Extension Method:
        /// This method returns a list of items that are contained in the string between the two separator.
        /// example:
        /// <code> "someValues : {{one}},{{two}},{{three}}".ExtractEntriesBetween("{{", "}}");</code>
        /// will return a list containing these items in a list:
        /// <code>
        /// one,two,three
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="startSeparator">the left separator</param>
        /// <param name="endSeparator">the right separator</param>
        /// <returns>a list with the items</returns>
        public static IList<string> ExtractValues(this String str, String startSeparator, String endSeparator)
        {
            if (str.IsNullOrEmpty())
            {
                return new List<string>();
            }
            int startIndex = str.IndexOf(startSeparator);
            IList<string> result = new List<string>();
            int maxLoop = 1;
            if (startIndex < 0)
            {
                return result;
            }

            while (startIndex >= 0 && maxLoop < 100)
            {
                maxLoop++;
                str = str.Remove(0, startIndex + startSeparator.Length);
                int endIndex = str.IndexOf(endSeparator);

                if (endIndex < 0)
                {
                    return result;
                }

                String item = str.Substring(0, endIndex);

                result.Add(item);
                str = str.Remove(0, endIndex + endSeparator.Length);
                startIndex = str.IndexOf(startSeparator);

            }
            return result;

        }

      
       
        /// <summary>
        /// Extension Method:
        /// This method returns a String that is contained in the string between the two separator.
        /// example:
        /// <code> "valueOut(valueIn)valueExt".BetweenFirst("(", ")",StringManipulationMode.Exclusive);</code>
        /// will return a String with:
        /// <code>
        /// valueIn
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="startSeparator">the left separator</param>
        /// <param name="endSeparator">the right separator</param>
        /// <param name="mode">exclude or include the separators</param>
        /// <returns>the extrated string</returns>
        public static String BetweenFirst(this String str, String from, String to, StringManipulationMode mode)
        {
            String result = null;
            int firstIndex = str.IndexOf(from);
            int lastIndex = str.IndexOf(to);

            if(lastIndex < 0 || firstIndex < 0 || lastIndex < firstIndex) {
                return result;
            }

            
            if(mode == StringManipulationMode.Exclusive){
             result = SubStringFrom(str, firstIndex + from.Length + 1, lastIndex);
            }
            else
                result = SubStringFrom(str, firstIndex + 1 , lastIndex + to.Length);
            return result;
        }

        public static String SubStringFrom(this String str, int from, int to)
        {
            return SubStringFrom(str, from, to, StringManipulationMode.Inclusive);
        }

        public static String SubStringFrom(this String str, int from, int to, StringManipulationMode mode)
        {
            if (str.Length < to || from > to)
            {
                throw new ArgumentException("from must be less or equal than to and string length must be greater or equal than to");
            }

            String result = str;

            if (mode == StringManipulationMode.Inclusive)
            {
                result = str.Substring(from - 1, to + 1 - from);
            }
            else if (mode == StringManipulationMode.Exclusive)
            {
                result = str.Substring(from, to -1 - from);
            }
            else
            {
                throw new ArgumentException(mode + " is not yet supported");
            }
            return result;
            

        }

        public static Boolean IsNullOrEmpty(this string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return true;
            }
            return false;
        }

        public static Boolean IsNotNullOrEmpty(this string str)
        {
            return !IsNullOrEmpty(str);
        }

      
        /// <summary>
        /// Extension Method:
        /// This method delete a substring between two separators and returns a new String.
        /// example:
        /// <code> "(one)(two)(three)".EraseBetweenFirst('(', ')', StringManipulationMode.Exclusive);</code>
        /// will return a String with:
        /// <code>
        /// ()(two)(three)
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="from">the left separator</param>
        /// <param name="to">the right separator</param>
        /// <param name="mode">exclude or include the separators</param>
        /// <returns>the new string</returns>
        public static string EraseBetweenFirst(this string str, char from, char to, StringManipulationMode mode)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }
            int startindex = (str).IndexOf(from);
            if (startindex < 0)
            {
                return str;
            }
            int endIndex = str.IndexOf(to);
            string result = null;

            if(mode == StringManipulationMode.Exclusive) 
            {
                result = str.Remove(startindex + 1, (endIndex + 1) - (startindex + 2));
            }
            else
            {
                result = str.Remove(startindex, (endIndex + 1 ) - (startindex));
            }
            return result;
        }

        

        /// <summary>
        /// Extension Method:
        /// This method return the substring before a separator
        /// example:
        /// <code> "before.value".BeforeLast('.', StringManipulationMode.Exclusive);</code>
        /// will return a String with:
        /// <code>
        /// before
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="from">the separator</param>
        /// <param name="mode">exclude or include the separators</param>
        /// <returns>the new string</returns>
        public static string BeforeLast(this string str, char from, StringManipulationMode mode)
        {
            int startindex = str.LastIndexOf(from);
            if (startindex < 0)
            {
                return str;
            }

            string result;
            if(mode == StringManipulationMode.Exclusive){
                 result = str.Substring(0, startindex);

            }
            else
            {
                 result = str.Substring(0, startindex+1);

            }
            
            return result;
        }


        /// <summary>
        /// Extension Method:
        /// This method return the substring after a separator
        /// example:
        /// <code> "before.value".BeforeLast('.', StringManipulationMode.Exclusive);</code>
        /// will return a String with:
        /// <code>
        /// before
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="from">the separator</param>
        /// <param name="mode">exclude or include the separators</param>
        /// <returns>the new string</returns>
        public static string AfterLast(this string str, char from, StringManipulationMode mode)
        {
            if (str == null)
            {
                return str;
            }

            int startindex = (str).LastIndexOf(from);
            if (startindex < 0)
            {
                return str;
            }

            string result;
            if (mode == StringManipulationMode.Exclusive)
            {
                result = str.Remove(0, startindex + 1);

            }
            else
            {
                result = str.Remove(0, startindex);

            }

            return result;
        }

        public static KeyValuePair<string, string> KeyValue(this string str, char splitter)
        {
            if (str.IsNullOrEmpty())
            {
                return new KeyValuePair<string,string>();
            }
            String[] keyValue = str.Split(splitter);
            return new KeyValuePair<string, string>(keyValue[0], keyValue[1]);
        }

        public static KeyValuePair<string, string> KeyValue(this string str)
        {
            return KeyValue(str, '=');
        }

        /// <summary>
        /// Extension Method:
        /// This method return a new string after removing a given substring
        /// <code> "value.after".RemoveLast("after");</code>
        /// will return a String with:
        /// <code>
        /// value.
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="toBeRemoved">the substring to remove</param>
        /// <returns>the new string</returns>
        public static string RemoveLast(this string str, string toBeRemoved)
        {
            if(str == null) return null;
            int startindex = (str).LastIndexOf(toBeRemoved);
            if (startindex < 0)
            {
                return str;
            }
            string result = str.Remove(startindex, toBeRemoved.Length);
            return result;
        }

        /// <summary>
        /// Extension Method:
        /// This method return a new string after removing after given substring
        /// <code>  "value.after".RemoveAfter("af");</code>
        /// will return a String with:
        /// <code>
        /// value.
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="toBeRemoved">the substring to remove</param>
        /// <returns>the new string</returns>
        public static string RemoveAfter(this string str, string toBeRemoved)
        {
            if (str == null) return null;
            int startindex = (str).LastIndexOf(toBeRemoved);
            if (startindex < 0)
            {
                return str;
            }
            string result = str.Remove(startindex, str.Length - startindex);
            return result;
        }
        /// <summary>
        /// Extension Method:
        /// This method return a new string after removing after given substring
        /// <code>  "value.after".RemoveBefore("af");</code>
        /// will return a String with:
        /// <code>
        /// ter
        /// </code>
        ///
        /// </summary>
        /// <param name="str">the string to act on</param>
        /// <param name="toBeRemoved">the substring to remove</param>
        /// <returns>the new string</returns>
        public static string RemoveBefore(this string str, string toBeRemoved)
        {
            
            if (str == null) return null;
            int startindex = (str).IndexOf(toBeRemoved);
            if (startindex < 0)
            {
                return str;
            }
            string result = str.Remove(0, startindex + toBeRemoved.Length);
            return result;
        }

        public static string[] Split(this string str, string splitter)
        {
            return str.Split(new string[] { splitter }, StringSplitOptions.RemoveEmptyEntries); 
        }


       public static string ReplaceExpressions(this string str,string enclosingStart = "{{", string enclosingEnd = "}}", Boolean concat = false)
       {
           if (str == null) return null;
           if(!str.Contains(enclosingStart))
           {
               return str;
           }
           string[] strEnclosure = str.SplitStringEnclosing();
          
           string expression = "";
           for (int i = 0; i < strEnclosure.Length; ++i)
           {
      
               if (concat)
               {
                   if (i > 0) expression = expression + " + ";
                   if (strEnclosure[i].IndexOf('{') != -1)
                   {
                      string  str2 = strEnclosure[i].RemoveEnclosure();
                      if (str2.Split(".").Length == 1) str2 =  str2 + ".Target";
                      expression = expression + str2;
                            
                   }
                   else expression = expression + "'" + strEnclosure[i] + "'";
               }

               else
               {
                   if (strEnclosure[i].IndexOf('{') != -1)
                   {
                       string str2 = strEnclosure[i].RemoveEnclosure();
                       if (str2.Split(".").Length == 1) str2 = str2 + ".Target";
                       expression = expression + str2;
                   }
                   else expression = expression + strEnclosure[i];
               }

           }

           return expression;
           }
       /// <summary>
       /// Extension Method:
       /// This method return a array of substrings of a string separated by values enclosed by a specific char
       /// <code>  "databases/{{config.General.DataBase}}/collections/{{resourceTemplate.Source}}".SplitString();</code>
       /// will return a String with:
       /// <code>
       /// [databases/,{{config.General.DataBase}},/collections/,{{resourceTemplate.Source}}]
       /// </code>
       ///
       /// </summary>
       /// <param name="str">the string to act on</param>
       /// <param name="enclosingStart">the enclosing start</param>
       /// <param name="enclosingStart">the enclosing start</param>
       /// <returns>array of substrings</returns>
       public static string[] SplitStringEnclosing(this string str, string enclosingStart = "{{", string enclosingEnd = "}}")
       {
           int[] startIndexs = str.IndexOfEnclosure(enclosingStart);
           int[] endIndexs = str.IndexOfEnclosure(enclosingEnd);
           IList<string> subString = new List<string>();
           int currentIndex = 1;
           int lastIndex = startIndexs[0];
           int i = 0;
           int j = 0;

           if ((startIndexs[0] + 1) == currentIndex)
           {
               lastIndex = endIndexs[0] + 2;
               i = 1;
           }
           
           while ((i <= startIndexs.Length * 2) && (currentIndex <= lastIndex))
           {
               
               subString.Add(str.SubStringFrom(currentIndex, lastIndex));
            
               currentIndex = lastIndex + 1;

               if (i % 2 == 0) lastIndex = endIndexs[j] + 2;
               else
               {
                   if (j < startIndexs.Length - 1)
                   {
                       ++j;
                       lastIndex = startIndexs[j];
                   }

                   else lastIndex = str.Length;
               }
                   ++i;

           }
           return subString.ToArray();
       }

       public static int[] IndexOfEnclosure(this string str, string enclosing)
       {
           IList<int> index = new List<int>();
           int i = 0;
           
           while ((i = str.IndexOf(enclosing, i)) != -1)
           {
               index.Add(i);
               ++i;
           }
          int[] array = index.ToArray();
           return array;
       }

        //TODO this method needs to be more general. right now only works with {{}}
        public static string RemoveEnclosure(this string str)
       {
           return str.SubStringFrom(3,str.Length-2);
       }

    }
}
