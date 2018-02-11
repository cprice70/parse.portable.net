//{
//  "username": "cooldude6",
//  "phone": "415-392-0202",
//  "createdAt": "2011-11-07T20:58:34.448Z",
//  "updatedAt": "2011-11-07T20:58:34.448Z",
//  "objectId": "g7y9tkhB7O",
//  "sessionToken": "r:pnktnjyb996sj4p156gjtp4im"
//}

namespace parse.portable.net.Rest
{
    /// <summary>The information for a single iOS device</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.22.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ParseUserRequest : System.ComponentModel.INotifyPropertyChanged
    {
        private string username;
        private string phone;
        private string createdAt;
        private string updatedAt;
        private string objectId;
        private string sessionToken;

        /// <summary>The Unique Device IDentifier of the device</summary>
        [Newtonsoft.Json.JsonProperty("username", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>The model identifier of the device, in the format iDeviceM,N</summary>
        [Newtonsoft.Json.JsonProperty("phone", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>The build number of the last known OS version running on the device</summary>
        [Newtonsoft.Json.JsonProperty("createdAt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CreatedAt
        {
            get { return createdAt; }
            set
            {
                if (createdAt != value)
                {
                    createdAt = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>The last known OS version running on the device</summary>
        [Newtonsoft.Json.JsonProperty("updatedAt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UpdatedAt
        {
            get { return updatedAt; }
            set
            {
                if (updatedAt != value)
                {
                    updatedAt = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>The device's serial number. Always empty or undefined at present.</summary>
        [Newtonsoft.Json.JsonProperty("objectId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ObjectId
        {
            get { return objectId; }
            set
            {
                if (objectId != value)
                {
                    objectId = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>The device's International Mobile Equipment Identity number. Always empty or undefined at present.</summary>
        [Newtonsoft.Json.JsonProperty("sessionToken", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SessionToken
        {
            get { return sessionToken; }
            set
            {
                if (sessionToken != value)
                {
                    sessionToken = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static ParseUserRequest FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ParseUserRequest>(data);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
