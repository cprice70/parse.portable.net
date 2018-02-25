using System;
using Newtonsoft.Json;

namespace parse.portable.net.Rest.Models
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

        [JsonProperty("objectId")]
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

        [JsonIgnore]
        public virtual string ClassName { get; }
    }
}