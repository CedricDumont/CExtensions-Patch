using CExtensions.Patch.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExtensions.Patch
{
    public class JsonPatchDocument : List<Operation>
    {
        private JsonPatchDocument() : base() {

            DirtyProperties = new List<string>();

        }

        private JsonPatchDocument(IEnumerable<Operation> operationList) : base(operationList) { }

        public static JsonPatchDocument FromJson(String jsonString)
        {
            var list = JsonConvert.DeserializeObject<IList<OperationTemplate>>(jsonString);

            var operationList = Operation.FromTemplateList(list);

            var jsonPatcDoc =  new JsonPatchDocument(operationList);
            jsonPatcDoc.DirtyProperties =  new List<string>();

            return jsonPatcDoc;

        }

        public static JsonPatchDocument Create()
        {
            return new JsonPatchDocument();
        }

        public Boolean IsInError { get; private set; }

        public dynamic Source { get; private set; }

        private List<String> DirtyProperties { get; set; }

        public IDictionary<string, object> Flatten(string jo)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            using (var _jsonTextReader = new JsonTextReader(new System.IO.StringReader(jo)))
            {
                {
                    while (_jsonTextReader.Read())
                    {
                        if (_jsonTextReader.TokenType == JsonToken.String ||
                                _jsonTextReader.TokenType == JsonToken.Integer ||
                                _jsonTextReader.TokenType == JsonToken.Float ||
                                _jsonTextReader.TokenType == JsonToken.Boolean ||
                                _jsonTextReader.TokenType == JsonToken.Date ||
                                _jsonTextReader.TokenType == JsonToken.Null)
                        {
                            if (dictionary.ContainsKey(_jsonTextReader.Path))
                            {
                                dictionary[_jsonTextReader.Path] = _jsonTextReader.Value;
                            }
                            else
                            {
                                dictionary.Add(_jsonTextReader.Path,(_jsonTextReader.Value));
                            }
                        }
                    }
                }
            }

            return dictionary;
        }

        public IDictionary<string, Object> OriginalValues { get; set; }


        public async Task ApplyTo(dynamic target, Boolean removeUntouched = false)
        {
            string copy = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(target));

            Source = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject(copy));

            OriginalValues = Flatten(copy);

            foreach (var operation in this)
            {
                operation.Target = target;

                operation.Execute();

                DirtyProperties.AddRange(operation.DirtyTokens);

                if (operation.IsInError)
                {
                    IsInError = true;
                    target = Source;
                    break;
                }
            }

            if (removeUntouched)
            {
                var toBeRemoved = (from original in OriginalValues.Keys where !DirtyProperties.Contains(original) orderby original descending select original ).ToList();

                foreach (var path in toBeRemoved)
                {
                    RemoveOperation op = new RemoveOperation() { Target = target,  Path = path };
                    op.Execute();
                }

          
            }

        }

        public JsonPatchDocument Add(String path, Object value)
        {
            var operation = new AddOperation() { Path = path, Value = value };
            this.Add(operation);
            return this;
        }

        public JsonPatchDocument Remove(String path)
        {
            var operation = new RemoveOperation() { Path = path };
            this.Add(operation);
            return this;
        }

        public JsonPatchDocument Copy(string from, string path)
        {
            var operation = new CopyOperation() { From = from, Path = path };
            this.Add(operation);
            return this;
        }

        public JsonPatchDocument Move(string from, string path)
        {
            var operation = new MoveOperation() { From = from, Path = path };
            this.Add(operation);
            return this;
        }

        public JsonPatchDocument Replace(string path, Object value)
        {
            var operation = new ReplaceOperation() { Path = path, Value = value };
            this.Add(operation);
            return this;
        }

        public JsonPatchDocument Test(string path, Object value)
        {
            var operation = new TestOperation() { Path = path, Value = value };
            this.Add(operation);
            return this;
        }

        public String Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in this)
            {
                sb.Append(item.ToTemplate().AsJson());
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }

        public override string ToString()
        {
            return Serialize();
        }
    }


}
