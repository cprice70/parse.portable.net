namespace parse.portable.net.Rest
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.22.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum ErrorDetailsCode
    {
        [System.Runtime.Serialization.EnumMember(Value = "BadRequest")]
        BadRequest = 0,
    
        [System.Runtime.Serialization.EnumMember(Value = "Conflict")]
        Conflict = 1,
    
        [System.Runtime.Serialization.EnumMember(Value = "NotAcceptable")]
        NotAcceptable = 2,
    
        [System.Runtime.Serialization.EnumMember(Value = "NotFound")]
        NotFound = 3,
    
        [System.Runtime.Serialization.EnumMember(Value = "InternalServerError")]
        InternalServerError = 4,
    
        [System.Runtime.Serialization.EnumMember(Value = "Unauthorized")]
        Unauthorized = 5,
    
        [System.Runtime.Serialization.EnumMember(Value = "TooManyRequests")]
        TooManyRequests = 6,
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.22.0 (Newtonsoft.Json v9.0.0.0)")]
    public class ErrorDetails : System.ComponentModel.INotifyPropertyChanged
    {
        private ErrorDetailsCode _code;
        private string _message;
    
        [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ErrorDetailsCode Code
        {
            get => _code;
            set 
            {
                if (_code == value) return;
                _code = value; 
                RaisePropertyChanged();
            }
        }
    
        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string Message
        {
            get => _message;
            set 
            {
                if (_message == value) return;
                _message = value; 
                RaisePropertyChanged();
            }
        }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static ErrorDetails FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorDetails>(data);
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}