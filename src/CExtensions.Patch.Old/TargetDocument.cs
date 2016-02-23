using CExtensions.Patch.Mapper.Patch.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils;
using CExtensions.Patch.Web.Mime;
using CExtensions.Patch.Utils.Serialization;
using Newtonsoft.Json;

namespace CExtensions.Patch.Mapper.Patch
{
    public class TargetDocument : IEnumerable<DocumentProperty>, IDisposable
    {
        private Dictionary<Path, DocumentProperty> _properties = new Dictionary<Path, DocumentProperty>();

        private System.IO.Stream _stream = null;

        private MediaType _inMediaType;

        private MediaType _outMediaType;

        private SerializationOptions _serializationOptions;

        private Boolean _isTokenized = false;

        private DisposableJsonTextReader _jsonTextReader;

        public TargetDocument(System.IO.Stream stream, MediaType inMediaType, MediaType outMediaType, SerializationOptions serializationOptions)
        {
            _stream = stream;
            _inMediaType = inMediaType;
            _outMediaType = outMediaType;
            _serializationOptions = serializationOptions;
        }

        public TargetDocument(System.IO.Stream stream, MediaType inMediaType, MediaType outMediaType)
            : this(stream, inMediaType, outMediaType, SerializationOptions.SurroundStringWithQuotes)
        {
        }

        public TargetDocument(System.IO.Stream stream, MediaType mediaType)
            : this(stream, mediaType, mediaType, SerializationOptions.SurroundStringWithQuotes)
        {
        }

        public TargetDocument(System.IO.Stream stream, MediaType mediaType, SerializationOptions serializationOptions)
            : this(stream, mediaType, mediaType, serializationOptions)
        {
        }

        public Object this[Path path]
        {
            get
            {
                return ValueOf(path);
            }
        }

        public Object ValueOf(String path)
        {
            return ValueOf(new Path(path));
        }

        public Object ValueOf(Path path)
        {
            return _properties[new Path(path)].Value;
        }

        public void Patch(PatchDocument patch)
        {
            if (!_isTokenized)
            {
                Tokenize(_stream);
            }

            //create a copy of the dictionary in case of error of the patch
            var backupProperties = _properties.ToDictionary(entry => entry.Key,
                                               entry => (DocumentProperty)entry.Value.Clone());

            foreach (var operation in patch.Operations)
            {
                Boolean result = operation.Apply(this);

                if (result == false)
                {
                    //rollback to previous properties
                    _properties = backupProperties;
                    throw new PatchException("Operation (" + operation.Name + "( for path (" + operation.Path + ") failed to execute");

                }
            }
        }

        public Boolean ContainsPath(String path)
        {
            return ContainsPath(new Path(path));
        }

        public Boolean ContainsPath(Path path)
        {
            return _properties.ContainsKey(path);
        }

        public IEnumerator<DocumentProperty> GetEnumerator()
        {
            return GetOrderedProperties().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetOrderedProperties().GetEnumerator();
        }

        private void Tokenize(System.IO.Stream stream)
        {
            if (!_isTokenized)
            {
                _jsonTextReader = new DisposableJsonTextReader(new System.IO.StreamReader(stream));
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
                            this.CreateOrUpdateProperty(new Path(_jsonTextReader.Path), _jsonTextReader.Value, false);
                        }
                    }
                }

                _isTokenized = true;
            }
        }

        private IEnumerable<DocumentProperty> GetOrderedProperties()
        {
            if (!_isTokenized)
            {
                Tokenize(_stream);
            }
            IEnumerable<DocumentProperty> result =
                from pair in _properties where pair.Value.Touched == _serializationOptions.onlyTouched orderby pair.Value.Path ascending select pair.Value;

            return result;
        }

        //public LinkedList<DocumentProperty> Properties
        //{
        //    get
        //    {
        //        LinkedList<DocumentProperty> result = new LinkedList<DocumentProperty>();

        //        foreach (var item in GetOrderedProperties())
        //        {
        //            result.AddLast(item);
        //        }

        //        return result;
        //    }
        //}

        public DocumentProperty GetPropertyAtPosition(int position)
        {
            return GetOrderedProperties().ElementAt(position);
        }

        public void CreateOrUpdateProperty(Path path, Object value, bool touch)
        {
            if (_properties.ContainsKey(path))
            {
                _properties[path] = new DocumentProperty(path, value, touch);
            }
            else
            {
                _properties.Add(path, new DocumentProperty(path, value, touch));
            }
        }


        public void RemoveProperty(Path path)
        {
            if (this.ContainsPath(path))
            {
                _properties.Remove(path);
            }
        }

        //public System.IO.Stream Serialize()
        //{
        //    _outStream = new MemoryStreamMultiplexer();
        //    Path previousPath = new Path("");
        //    Path currentPath = Path.Empty;
        //    StackSerializer serializer = null;

        //    foreach (var property in this.GetOrderedProperties())
        //    {
        //        currentPath = property.Path;
        //        serializer = new StackSerializer(previousPath, currentPath, property.Value, _serializationOptions);
        //        _outStream.Write(serializer.Result);
        //        previousPath = currentPath;

        //    }
        //    //last Path is empty
        //    currentPath = Path.Empty;
        //    serializer = new StackSerializer(previousPath, currentPath, null, _serializationOptions);

        //    _outStream.Write(serializer.Result);

        //    _outStream.Finish();

        //    return _outStream.GetReader();

        //}

        public void Dispose()
        {
            //_outStream.Finish();
            if (_jsonTextReader != null)
            {
                _jsonTextReader.Dispose();
            }
            //if (_outStream != null)
            //{
            //    _outStream.Dispose();
            //}
        }

        private class DisposableJsonTextReader : JsonTextReader
        {
            private Boolean _disposed;

            internal DisposableJsonTextReader(System.IO.StreamReader stream) : base(stream) { }

            internal void Dispose()
            {
                if (!_disposed)
                {
                    base.Dispose(true);
                    _disposed = true;
                }
            }
        }
    }


}
