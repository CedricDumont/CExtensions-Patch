using CExtensions.Patch.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CExtensions.Patch.Utils
{
    public struct Path : IEquatable<Path>, IComparable<Path>, IEnumerable<PathMetaData>
    {
        private const string PATH_SEPARATOR = ".";

        private const char PATH_SEPARATOR_CHAR = '.';

        private readonly string _name;

        private readonly string[] _siblings;

        private readonly LinkedList<PathMetaData> _metaDataList;

        public static readonly Path Empty = new Path("");


        public Path(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("constructor parameter should not be null");
            }
            _name = name;
            _siblings = name.Split(PATH_SEPARATOR_CHAR);
            _metaDataList = new LinkedList<PathMetaData>();
            buildMetaData();
        }

        private void buildMetaData()
        {
            int siblings_totalCount = _siblings.Length;
            int counter = 0;
            foreach (var sibling in _siblings)
            {
                PathMetaData metadata = new PathMetaData();
                string name = sibling;
                if (sibling.EndsWith("]"))
                {
                    metadata.IsArray = true;
                    string index = sibling.BetweenFirst("[", "]", StringManipulationMode.Exclusive);
                    metadata.ArrayIndex = Int32.Parse(index);
                    name = sibling.RemoveAfter("[");
                }
                else if (counter < (_siblings.Length - 1))
                {
                    metadata.IsObject = true;
                }
                metadata.Name = name;

                counter++;
                _metaDataList.AddLast(metadata);
            }
        }

        public Boolean IsComplex
        {
            get
            {
                return _siblings.Length > 0;
            }
        }


        public String FirstSibling
        {
            get
            {
                return _siblings[0];
            }
        }

        public Boolean IsIndexer
        {
            get
            {
                return PropertyName.EndsWith("]");
            }
        }

        public int CurrentArrayIndex
        {
            get
            {
                try
                {
                    string sindex = PropertyName.BetweenFirst("[", "]", StringManipulationMode.Exclusive);

                    return Int32.Parse(sindex);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public Path Root
        {
            get
            {
                if (_siblings == null)
                {
                    return Empty;
                }
                StringBuilder buildPath = new StringBuilder();
                for (int i = 0; i < _siblings.Length - 1; i++)
                {
                    buildPath.Append(_siblings[i]);
                    buildPath.Append(PATH_SEPARATOR);
                }
                buildPath.RemoveLast();
                return new Path(buildPath.ToString());
            }
        }

        public String PropertyName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Root))
                {
                    return _name.AfterLast('.', StringManipulationMode.Exclusive);
                    //string temp = _name.Replace(this.Root, "");
                    //return temp.Replace(PATH_SEPARATOR, "");
                }
                return this._name;
            }
        }

        public String ObjectName
        {
            get
            {
                if (this.IsIndexer)
                {
                    String result = this.PropertyName.RemoveAfter("[");
                    return result;
                }
                else
                {
                    return PropertyName;
                }

            }
        }

        public Boolean HasSameRoot(Path path)
        {
            Path thisPathWithoutEnd = this.Root;
            Path paramPathWithoutEnd = path.Root;

            return thisPathWithoutEnd == paramPathWithoutEnd;
        }

        public int Depth
        {
            get
            {
                return _siblings.Length;
            }
        }

        public String this[int index]
        {
            get
            {
                return _siblings[index];
            }
        }


        public static explicit operator Path(string name)
        {
            Path f = new Path(name);

            return f;
        }

        public static implicit operator string(Path f)
        {
            return f._name.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Path)
            {
                return this.Equals((Path)obj);
            }
            return false;
        }

        public bool Equals(Path p)
        {
            return (_name == p);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode(); ;
        }

        public static bool operator ==(Path lhs, Path rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Path lhs, Path rhs)
        {
            return !(lhs.Equals(rhs));
        }

        public override string ToString()
        {
            return _name;
        }

        public int CompareTo(Path other)
        {
            return this._name.CompareTo(other._name);
        }

        public IEnumerator<PathMetaData> GetEnumerator()
        {
            return _metaDataList.GetEnumerator();
            //return _siblings.ToList<string>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _metaDataList.GetEnumerator();
            //return _siblings.GetEnumerator();
        }

        public LinkedList<PathMetaData> Parts
        {
            get
            {
                return _metaDataList;
            }
        }
    }
}
