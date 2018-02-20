using Newtonsoft.Json;

namespace parse.portable.net.Rest.Models
{
    public class ParseResponse
    {
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("objectId")]
        public string ObjectId { get; set; }
    }
}
