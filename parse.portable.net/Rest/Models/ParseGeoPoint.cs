using Newtonsoft.Json;
using parse.portable.net.Rest.Models;

namespace Parse.Api.Models
{
    /// <summary>
    /// Parse data type for a geographic point (lat + lon)
    /// </summary>
    public class ParseGeoPoint
    {
        public ParseGeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        private const string ParseType = "GeoPoint";

        [JsonProperty(ParseObject.TypeProperty)]
        internal readonly string Type = ParseType;

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}