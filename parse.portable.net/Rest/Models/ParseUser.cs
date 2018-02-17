using System;
using Newtonsoft.Json;
using parse.portable.net.Models;
using Parse.Api.Attributes;

namespace parse.portable.net.Rest.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Default Parse User, should be inherited for custom User classes (i.e. if "phoneNumber" is added)
    /// </summary>
    public class ParseUser : ParseObject
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("sessionToken")]
        public string SessionToken { get; set; }

        [JsonIgnoreForSerialization]   
        public bool? EmailVerified { get; set; }

        [JsonIgnoreForSerialization]   
        public AuthData AuthData { get; set; }
        public override string ClassName => string.Empty;

    }

    public class AuthData
    {
        [JsonProperty("anonymous")]
        public AnonAuthData Anonymous { get; set; }
    }

    public class FacebookAuthData
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class TwitterAuthData
    {
        public string Id { get; set; }
        public string ScreenName { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AuthToken { get; set; }
        public string AuthTokenSecret { get; set; }
    }

    public class AnonAuthData
    {
        public string Id { get; set; }
    }
}