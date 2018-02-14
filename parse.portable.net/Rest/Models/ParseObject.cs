using System;
using parse.portable.net.Rest.Models;
using Parse.Api.Attributes;
using Parse.Api.Models;

namespace parse.portable.net.Models
{
    /// <summary>
    /// Base class for all objects in Parse
    /// </summary>
    public class ParseObject
    {
        [JsonIgnoreForSerialization]
        public DateTime CreatedAt { get; set; }

        [JsonIgnoreForSerialization]
        public DateTime UpdatedAt { get; set; }

        [JsonIgnoreForSerialization]
        public string ObjectId { get; set; }

        internal static string GetClassName(Type type)
        {
            return typeof(ParseUser).IsAssignableFrom(type) ? "_User" : type.Name;
        }

        internal const string TypeProperty = "__type";
    }
}