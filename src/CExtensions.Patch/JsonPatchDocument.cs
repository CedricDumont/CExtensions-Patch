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
        private JsonPatchDocument(JsonPatchOptions options = null) 
        {
            DirtyProperties = new List<string>();
            Options = options ?? JsonPatchOptions.Default;
        }

        private JsonPatchDocument(IEnumerable<Operation> operationList, JsonPatchOptions options = null) : base(operationList)
        {
            DirtyProperties = new List<string>();
            Options = options ?? JsonPatchOptions.Default;
        }

        public static JsonPatchDocument FromJson(String jsonString)
        {
            var list = JsonConvert.DeserializeObject<IList<OperationTemplate>>(jsonString);

            var operationList = Operation.FromTemplateList(list);

            var jsonPatcDoc =  new JsonPatchDocument(operationList);

            return jsonPatcDoc;
        }


        public static JsonPatchDocument Create()
        {
            return new JsonPatchDocument();
        }

        public JsonPatchOptions Options
        {
            get; internal set;
        }

        public Boolean IsInError { get; private set; }

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

        public string _originalDocument;
        public String OriginalDocument
        {
            get
            {
                if(!Options.TraceOriginalDocument)
                {
                    throw new Exception("Enable TraceOriginalDocument using options.");
                }
                return _originalDocument;
            }
            private set
            {
                _originalDocument = value;
            }
        } 

        public async Task<String> ApplyTo(String json)
        {
            var result = await ApplyTo(JObject.Parse(json));

            return JsonConvert.SerializeObject(result);
        }

        public async Task<Object> ApplyTo(dynamic target)
        {
            if (Options.TraceOriginalDocument)
            {
                OriginalDocument = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(target));
            }

            foreach (var operation in this)
            {
                operation.Target = target;

                operation.Execute();

                DirtyProperties.AddRange(operation.DirtyTokens);

                if (operation.IsInError && Options.RecoverIfError)
                {
                    IsInError = true;
                    target = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject(OriginalDocument));
                    break;
                }
            }

            if (Options.RemoveUntouched)
            {
                OriginalValues = Flatten(OriginalDocument);

                var toBeRemoved = (from original in OriginalValues.Keys where !DirtyProperties.Contains(original) orderby original descending select original ).ToList();

                foreach (var path in toBeRemoved)
                {
                    RemoveOperation op = new RemoveOperation() { Target = target,  Path = path };
                    op.Execute();
                }
            }

            return target;
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
