using parse.portable.net.Rest;

namespace ParseCommon.Rest
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "9.10.22.0 (Newtonsoft.Json v9.0.0.0)")]
    public sealed class ErrorResponse : System.ComponentModel.INotifyPropertyChanged
    {
        private ErrorDetails _error;
    
        [Newtonsoft.Json.JsonProperty("error", Required = Newtonsoft.Json.Required.Always)]
        public ErrorDetails Error
        {
            get => _error;
            set 
            {
                if (_error == value) return;
                _error = value; 
                RaisePropertyChanged();
            }
        }
    
        public string ToJson() 
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        
        public static ErrorResponse FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(data);
        }
    
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}