using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace parse.portable.net.Models
{
    public class ParseResponse
    {
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("objectId")]
        public string ObjectId { get; set; }
    }
}
