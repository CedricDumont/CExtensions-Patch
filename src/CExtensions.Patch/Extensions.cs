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
        //public static Object Patch(this Object target, JsonPatchDocument patchDocument)
        //{
        //    var temp = JObject.FromObject(target);
        //    patchDocument.ApplyTo(temp);
        //    return temp;
        //}

        //public static string Patch(this String jsonObject, JsonPatchDocument patchDocument)
        //{
        //    var target = JObject.Parse(jsonObject);
        //    patchDocument.ApplyTo(target);
        //    return JsonConvert.SerializeObject(target);
        //}

    }
}
