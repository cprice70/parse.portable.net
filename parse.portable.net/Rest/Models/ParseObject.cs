using System;
using Newtonsoft.Json;
using parse.portable.net.Rest.Models;
using Parse.Api.Attributes;

namespace parse.portable.net.Models
{
    /// <summary>
    /// Base class for all objects in Parse
    /// </summary>
    public class ParseObject
    {
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set;  }

        [JsonIgnore]
        public string ObjectId { get; set; }

        public bool ShouldSerializeCreatedAt()
        {
            // never serialize CreatedAt
            return false;
        }

        public bool ShouldSerializeUpdatedAt()
        {
            return false;
        }

        internal static string GetClassName(Type type)
        {
            return typeof(ParseUser).IsAssignableFrom(type) ? "_User" : type.Name;
        }

        internal const string TypeProperty = "__type";

        public virtual string ClassName { get; }
    }
}