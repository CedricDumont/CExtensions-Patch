using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CExtensions.Patch.Utils.Extensions;

namespace CExtensions.Patch.Web.Mime
{
    public struct    MediaType
    {
        public static readonly String[] Names = new String[]{
               "application/json",
               "application/LD+json",
               "application/octet-stream",
               "application/xml",
               "application/csv",
               "media/undefined",
               "application/x-www-form-urlencoded",
               "text/plain",
               "TOBEDEF"
           };

        public static readonly MediaType Json = new MediaType("application/json");

        public static readonly MediaType JsonLD = new MediaType("application/LD+json");

        public static readonly MediaType Octet = new MediaType("application/octet-stream");
        
        public static readonly MediaType Object = Octet;

        public static readonly MediaType Xml = new MediaType("application/xml");

        public static readonly MediaType Csv = new MediaType("application/csv");

        public static readonly MediaType XFormUrlEncoded = new MediaType("application/x-www-form-urlencoded");

        public static readonly MediaType Undefined = new MediaType("media/undefined");

        public static readonly MediaType TextPlain = new MediaType("text/plain");

        public static readonly MediaType Atom = new MediaType("TOBEDEF");
         
        

        private String _mediaTypeName;

        private MediaType(string mediaTypeName)
        {
            if (!mediaTypeName.OneOf(Names))
            {
                mediaTypeName = mediaTypeName ?? "null";
                throw new ArgumentException("the Media " + mediaTypeName + " is not supported try with one of :" + Names.Flatten(','));
            }
            _mediaTypeName = mediaTypeName;
        }

        public override bool Equals(object obj)
        {
            if (obj is MediaType)
            {
                return this.Equals((MediaType)obj);
            }
            return false;
        }

        public static implicit operator string(MediaType rn)
        {
            return rn._mediaTypeName.ToString();
        }

        public static implicit operator MediaType(string rn)
        {
            return new MediaType(rn);
        }

        public bool Equals(MediaType p)
        {
            return (_mediaTypeName == p._mediaTypeName);
        }

        public override int GetHashCode()
        {
            return _mediaTypeName.GetHashCode(); ;
        }

        public static bool operator ==(MediaType lhs, MediaType rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MediaType lhs, MediaType rhs)
        {
            return !(lhs.Equals(rhs));
        }

        public override string ToString()
        {
            return _mediaTypeName;
        }
    }
}
