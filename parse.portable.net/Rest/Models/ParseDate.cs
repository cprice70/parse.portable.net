using System;
using Newtonsoft.Json;

namespace parse.portable.net.Rest.Models
{
    public partial class Date
    {
        [JsonProperty("__type")]
        public string Type  => "Date";

        [JsonProperty("iso")]
        public DateTime Iso { get; set; }
    }

}
