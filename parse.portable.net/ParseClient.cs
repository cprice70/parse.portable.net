using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using parse.portable.net.Rest.Models;
using Parse.Api;

namespace parse.portable.net
{
    public class ParseClient
    {
        private readonly Lazy<JsonSerializerSettings> _settings;

        private string AddId { get; }
        private string BaseUrl { get; }
        private string ClientKey { get; }

        public ParseClient(string appId, string baseUrl, string clientKey = "")
        {
            AddId = appId;
            BaseUrl = baseUrl;
            ClientKey = clientKey;

            _settings = new Lazy<JsonSerializerSettings>(() =>
            {
                var settings = new JsonSerializerSettings();
                //UpdateJsonSerializerSettings(settings);
                return settings;
            });
        }

        private void UpdateJsonSerializerSettings(ParseClient instance,
            JsonSerializerSettings settings)
        {
        }

        private void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
        }

        private void PrepareRequest(HttpClient client, HttpRequestMessage request,
            System.Text.StringBuilder urlBuilder)
        {
        }

        private void ProcessResponse(HttpClient client, HttpResponseMessage response)
        {
        }


        public async Task<bool> SignUp(string newUsername, string newPassword, CancellationToken token)
        {
            try
            {
                var signUpUrl = BaseUrl + ParseUrls.User;
                var getResp = await signUpUrl
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .PostUrlEncodedAsync(new
                    {
                        username = newUsername,
                        password = newPassword
                    }, token);

                return getResp.IsSuccessStatusCode;
            }
            catch (FlurlHttpTimeoutException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async Task<ParseUser> LoginAsync(string pUsername, string pPassword,
            CancellationToken cancellationToken, int timeoutSec = 30)
        {
            try
            {
                var loginUrl = BaseUrl + ParseUrls.Login;
                var getResp = await loginUrl
                    .WithTimeout(timeoutSec)
                    .SetQueryParams(new
                    {
                        username = pUsername,
                        password = pPassword
                    })
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .GetJsonAsync<ParseUser>(cancellationToken);
                
                return getResp;
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<ParseUser> LoginAnonymousAsync(
            CancellationToken cancellationToken, int timeoutSec = 30)
        {
            try
            {
                var parseAuth = new ParseAnonymousUtils.ParseAnonClass
                {
                    AuthData = new ParseAnonymousUtils.AuthData()
                };
                parseAuth.AuthData.Anonymous = new ParseAnonymousUtils.Anonymous
                {
                    Id = Guid.NewGuid().ToString()
                };
                var authJson = JsonConvert.SerializeObject(parseAuth);
                var loginUrl = BaseUrl + ParseUrls.User;
                var getResp = await loginUrl
                     .WithTimeout(timeoutSec)
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .PostJsonAsync(parseAuth, cancellationToken)
                    .ReceiveJson<ParseUser>();

                return getResp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> ValidateTokenAsync(string authToken, CancellationToken token, int timeoutSec = 30)
        {
            try
            {
                var validateUrl = BaseUrl + ParseUrls.Validate;
                var getResp = await validateUrl
                     .WithTimeout(timeoutSec)
                    .WithHeader("X-Parse-Session-Token", authToken)
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .GetAsync(token)
                    .ReceiveJson<ParseResponse>();

                    return (int)getResp.StatusCode != 209;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string emailaddress, CancellationToken token)
        {
            try
            {
                var resetUrl = BaseUrl + ParseUrls.PasswordReset;
                var getResp = await resetUrl
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .PostUrlEncodedAsync(new
                    {
                        email = emailaddress
                    }, token);

                return getResp.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Updates a pre-existing ParseUser
        /// </summary>
        /// <param name="user">The user to delete</param>
        /// <param name="sessionToken">Session token given by SignUp or LogIn</param>
        /// <param name="token"></param>
        public async Task<bool> DeleteUser<T>(T user, string sessionToken, CancellationToken token) where T : ParseUser, new()
        {
            if (user == null || string.IsNullOrEmpty(user.ObjectId) || string.IsNullOrEmpty(sessionToken)) throw new ArgumentException("ObjectId and SessionToken are required.");

            var resource = string.Format(ParseUrls.UserObject, user.ObjectId);
            var url = BaseUrl + resource;
            var resp = await url
                .WithHeader(ParseHeaders.AppId, AddId)
                .WithHeader("X-Parse-Revocable-Session", 1)
                //.WithHeader("Content-Type", "application/json")
                .WithHeader(ParseHeaders.SessionToken, sessionToken)
                .DeleteAsync(token);

            return resp.IsSuccessStatusCode;
        }

        /// <summary>
        /// Creates a new ParseObject
        /// </summary>
        /// <param name="obj">The object to be created on the server</param>
        /// <param name="className"></param>
        /// <param name="token"></param>
        /// <returns>A fully populated ParseObject, including ObjectId</returns>
        public async Task<T> CreateObjectAsync<T>(string className, object obj, CancellationToken token) where T : ParseObject, new()
        {
            if (string.IsNullOrWhiteSpace(className)) throw new ArgumentNullException(nameof(className));

            if (obj == null) throw new ArgumentNullException(nameof(obj));

            try
            {
                var createUrl = BaseUrl + string.Format(ParseUrls.Class, className);
                var getResp = await createUrl
                    .WithTimeout(10)
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .PostJsonAsync(obj, token)
                    .ReceiveJson<T>();

                if (getResp == null) return null;
                getResp.UpdatedAt = getResp.CreatedAt;
                return getResp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }  
        }

        public async Task<IList<T>> QueryObjectAsync<T>(string className, string query, CancellationToken token) where T : ParseObject, new()
        {
            object query_params = new { where=query };
            return await QueryObjectAsync<T>(className, query_params, token);
        }

        public async Task<IList<T>> QueryObjectAsync<T>(string className, string query, int querylimit, string orderBy, CancellationToken token) where T : ParseObject, new()
        {
			object query_params = new { where=query };// limit = querylimit };//, order = orderBy };
            return await QueryObjectAsync<T>(className, query_params, token);
        }

        public async Task<IList<T>> QueryObjectAsync<T>(string className, string query, int querylimit, CancellationToken token) where T : ParseObject, new()
        {
            object query_params = new { where=query, limit=querylimit};
            return await QueryObjectAsync<T>(className, query_params, token);
        }
        public async Task<IList<T>> QueryObjectAsync<T>(string className, object query_params, CancellationToken token) where T : ParseObject, new()        
        {
            if (string.IsNullOrWhiteSpace(className)) throw new ArgumentNullException(nameof(className));

            try
            {
                if (query_params == null) return null;

                var createUrl = BaseUrl + string.Format(ParseUrls.Class, className);
                var getResp = await createUrl
                    .SetQueryParams(query_params)
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .GetAsync(token)
                    .ReceiveString();
                var mDeserializeObject = JsonConvert.DeserializeObject<Results<T>>(getResp);
                return mDeserializeObject.results;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<T> CallFunctionAsync<T>(string name,
            IDictionary<string, object> parameters, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var config = new JsonSerializerSettings();
            //var jsonParams = JsonConvert.SerializeObject(parameters);
            try
            {
                var functionUrl = BaseUrl + string.Format(ParseUrls.Function, name);
                var getResp = await functionUrl
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .PostJsonAsync(parameters, token)
                    .ReceiveJson<T>();

                if (getResp == null) return default(T);
               
                return getResp;
            }
            catch (FlurlHttpException ex)
            {
                Console.WriteLine(ex);
                return default(T);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }

        private class ParseResponse
        {
            public string Content { get; set; }
            public HttpStatusCode StatusCode { get; set; }
        }

        private class Results<T>
        {
            [JsonProperty("results")]
            public T[] results { get; set; }
        }
    }
}
