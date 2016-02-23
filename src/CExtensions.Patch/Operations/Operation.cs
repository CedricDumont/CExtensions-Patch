using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch.Operations
{
    public abstract class Operation
    {
        public Operation(String name)
        {
            this.DirtyTokens = new List<string>();
            this.Name = name;
        }

        public dynamic Target { get; set; }

        public dynamic Source { get; set; }

        public dynamic Destination { get; set; }

        public String Path { get; set; }

        public Object Value { get; set; }

        public String Name { get; set; }

        public String From { get; set; }

        public  OperationTemplate ToTemplate()
        {
            OperationTemplate template = new OperationTemplate();

            template.op = this.Name;
            template.path = this.Path;
            template.from = this.From;
            template.value = this.Value;

            return template;
        }

        public static Operation FromTemplate(OperationTemplate template)
        {
            Operation op = null;

            if (template.op == "add")
            {
                op = new AddOperation();
            }
            else if (template.op == "remove")
            {
                op = new RemoveOperation();
            }
            else if (template.op == "test")
            {
                op = new TestOperation();
            }
            else if (template.op == "move")
            {
                op = new MoveOperation();
            }
            else if (template.op == "copy")
            {
                op = new CopyOperation();
            }
            else if (template.op == "replace")
            {
                op = new ReplaceOperation();
            }

            op.CopyPropertiesFrom(template);

            return op;
        }

        public static List<Operation> FromTemplateList(IEnumerable<OperationTemplate> templateList)
        {
            List<Operation> result = new List<Operation>();

            foreach (var template in templateList)
            {
                result.Add(Operation.FromTemplate(template));
            }

            return result;
        }

        private void CopyPropertiesFrom(OperationTemplate template)
        {
            this.Name = template.op;
            this.Path = template.path;
            this.From = template.from;
            this.Value = template.value;
        }

        public override string ToString()
        {
            return $"'op':'{Name}', 'path':'{Path}', 'value':'{Value}'";
        }

        public Boolean IsInError { get; set; }

        protected abstract void DoExecute();

        public virtual void Execute()
        {
            
            if (Path != null)
            {
                JToken tokenPath = Target.SelectToken(Path);

                if (tokenPath != null)
                {
                    DirtyTokens.Add(tokenPath.Path);
                }
            }

            if (From != null)
            {
                JToken tokenFrom = Target.SelectToken(From);
                if (tokenFrom != null)
                {
                    DirtyTokens.Add(tokenFrom.Path);
                }
            }

            DoExecute();
        }

        public List<string> DirtyTokens { get; set; }

        protected void AddPath(dynamic obj, string path, Object val)
        {
            //start with the target 
            JToken jpart = obj;

            //iterate the path and create the end part or the path
            foreach (var part in path.Split('.'))
            {
                if (part == "$") continue; // ignore root part

                string selector = part;

                if (part.EndsWith("]"))
                {
                    selector = part.Remove(part.IndexOf('['));
                }
                if (jpart.SelectToken(selector) == null)
                {
                    if (part.EndsWith("]"))
                    {
                        //Create an array with one empty object
                        JArray array = new JArray();
                        array.Add(new JObject());
                        ((JContainer)jpart).Add(new JProperty(selector, array));
                    }
                    else {
                        //create an empty object
                        ((JContainer)jpart).Add(new JProperty(selector, new JObject()));
                    }
                }

                jpart = (JToken)jpart.SelectToken(part);

                if (jpart == null)
                {
                    jpart = obj.SelectToken(selector);
                }
            }

            JToken jval = null;

            if (val != null)
            {
                if (!(val is JValue))
                {
                    if (!val.ToString().StartsWith("[") && !val.ToString().StartsWith("{"))
                        val = string.Format("\"{0}\"", val);


                    jval = (JToken)JObject.Parse(string.Format("{{\"x\":{0}}}", val)).SelectToken("x");
                }
                else
                {
                    jval = (JToken)val;
                }
            }
            else
            {
                jval = null;
            }

            if (IsArray(path))
            {
                JToken tokenfromPath = obj.SelectToken(path);
                if (tokenfromPath == null)
                {
                    ((JArray)jpart).Add(jval);
                }
                else {
                    jpart.AddBeforeSelf(jval);
                }
            }
            else {
                jpart.Replace(jval);
            }
        }

        protected bool IsArray(String path)
        {
            if(path.EndsWith("]"))
            {
                return true;
            }
            return false;
        }
    }
}
