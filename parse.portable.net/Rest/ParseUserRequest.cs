
namespace parse.portable.net.Rest
{
    /// <inheritdoc />
    /// <summary>The information for a single iOS device</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.22.0 (Newtonsoft.Json v9.0.0.0)")]
    public sealed class ParseUserRequest : System.ComponentModel.INotifyPropertyChanged
    {
        private string _username;
        private string _phone;
        private string _createdAt;
        private string _updatedAt;
        private string _objectId;
        private string _sessionToken;

        /// <summary>The Unique Device IDentifier of the device</summary>
        [Newtonsoft.Json.JsonProperty("username", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>The model identifier of the device, in the format iDeviceM,N</summary>
        [Newtonsoft.Json.JsonProperty("phone", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone == value) return;
                _phone = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>The build number of the last known OS version running on the device</summary>
        [Newtonsoft.Json.JsonProperty("createdAt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt == value) return;
                _createdAt = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>The last known OS version running on the device</summary>
        [Newtonsoft.Json.JsonProperty("updatedAt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UpdatedAt
        {
            get => _updatedAt;
            set
            {
                if (_updatedAt == value) return;
                _updatedAt = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>The device's serial number. Always empty or undefined at present.</summary>
        [Newtonsoft.Json.JsonProperty("objectId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ObjectId
        {
            get => _objectId;
            set
            {
                if (_objectId == value) return;
                _objectId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>The device's International Mobile Equipment Identity number. Always empty or undefined at present.</summary>
        [Newtonsoft.Json.JsonProperty("sessionToken", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SessionToken
        {
            get => _sessionToken;
            set
            {
                if (_sessionToken == value) return;
                _sessionToken = value;
                RaisePropertyChanged();
            }
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static ParseUserRequest FromJson(string data) => Newtonsoft.Json.JsonConvert.DeserializeObject<ParseUserRequest>(data);

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
