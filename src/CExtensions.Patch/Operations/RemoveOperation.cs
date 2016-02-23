using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public class RemoveOperation : Operation
    {
        public RemoveOperation() : base("remove") { }

        protected override void DoExecute()
        {
            JToken existing_Token = Target.SelectToken(Path);

            JToken parent = null;

            if (existing_Token != null)
            {
                parent = existing_Token.Parent;

                if (existing_Token is JObject)
                {
                    existing_Token.Remove();
                }
                else
                {
                    if (IsArray(Path))
                    {
                        existing_Token.Remove();
                    }
                    else {
                        parent = existing_Token.Parent?.Parent?.Parent;
                        existing_Token.Parent.Remove();
                    }
                }
            }

            if (parent != null)
            {
                if (parent is JArray)
                {
                    //check if the array contains empty objects
                    var next = parent.First;

                    while (next != null)
                    {
                        if (next is JObject && !next.HasValues)
                        {
                            next.Remove();
                        }
                        next = next.Next;
                    }
                    if (!parent.HasValues)
                    {
                        parent.Parent.Remove();
                    }
                }
                else if (!parent.HasValues)
                {
                    parent.Remove();
                }
                else
                {
                    if (parent.Count() == 1)
                    {
                        if (!parent.First.HasValues)
                        {
                            parent.Remove();
                        }
                    }
                }
            }

            //if (parent != null)
            //{
            //    if (parent.HasValues)
            //    {
            //       if(parent.Count() == 1)
            //        {
            //           if(!parent.First.HasValues)
            //            {
            //                parent.Remove();
            //            }
            //        }   
            //       if(parent is JArray)
            //        {
            //            foreach (var item in parent)
            //            {
            //               if(!item.HasValues)
            //                {
            //                    item.Remove();
            //                }
            //            }
            //            if(!parent.HasValues)
            //            {
            //                parent.Remove();
            //            }
            //        }      
            //    }
            //    else
            //    {
            //        parent.Remove();
            //    }
            //}
        }
    }
}
