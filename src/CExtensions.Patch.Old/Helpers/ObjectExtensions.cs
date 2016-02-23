using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static U NullsafeGet<T, U>(this T t, Func<T, U> fn)
        {
            if (t != null)
                return fn(t);
            else
                return default(U);
        }

        public static String AsString(this Object obj){
            if(obj == null)
            {
                return null;
            }
            return obj.ToString();
        }

        public static Stream AsStream(this Object obj)
        {
            if (obj == null)
            {
                return null;
            }
           
            MemoryStream m = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(m, obj);
            m.Position = 0;
            return m;
        }

        public static Boolean OneOf<T>(this Object obj, T[] list) where T : class
        {
            if (obj == null)
            {
                return false;
            }

            foreach (T item in list)
            {
                if((obj as T).Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static Boolean OneOf(this short obj, short[] list)
        {
            foreach (short item in list)
            {
                if (obj == item)
                {
                    return true;
                }
            }
            return false;
        }

        public static Object DefaultIfNull(this object obj, String propertyName)
        {
            if (obj == null)
            {
                return null;
            }

            Object val = null;
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(obj)[propertyName];
            
            if (descriptor != null)
            {
                val = descriptor.GetValue(obj);
                if (val == null)
                {
                    AttributeCollection attributes = descriptor.Attributes;
                    DefaultValueAttribute myAttribute =
                            (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
                    if (myAttribute != null)
                    {
                        val = myAttribute.Value;
                    }
                }
            }

            return val;
        }


        public static Boolean IsNotNull(this Object obj)
        {
            if(obj != null)
            {
                return true;
            }
            return false;
        }

        public static Boolean IsNull(this Object obj)
        {
            return !IsNotNull(obj);
        }

        public static Boolean IsNotTrue(this Boolean val)
        {
            if (!val)
            {
                return true;
            }
            return false;
        }

       
    }
}
