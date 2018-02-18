using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using parse.portable.net.Models;
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

        protected virtual void UpdateJsonSerializerSettings(ParseClient instance,
            JsonSerializerSettings settings)
        {
        }

        protected virtual void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
        }

        protected virtual void PrepareRequest(HttpClient client, HttpRequestMessage request,
            System.Text.StringBuilder urlBuilder)
        {
        }

        protected virtual void ProcessResponse(HttpClient client, HttpResponseMessage response)
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async Task<ParseUser> LoginAsync(string pUsername, string pPassword,
            CancellationToken cancellationToken)
        {
            try
            {
                var loginUrl = BaseUrl + ParseUrls.Login;
                var getResp = await loginUrl
                    .SetQueryParams(new
                    {
                        username = pUsername,
                        password = pPassword
                    })
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .GetJsonAsync<ParseUser>(cancellationToken);


                return getResp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<ParseUser> LoginAnonymousAsync(
            CancellationToken cancellationToken)
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

        public async Task<bool> ValidateTokenAsync(string authToken, CancellationToken token)
        {
            try
            {
                var validateUrl = BaseUrl + ParseUrls.Validate;
                var getResp = await validateUrl
                    .WithHeader("X-Parse-Session-Token", authToken)
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .GetAsync(token)
                    .ReceiveJson<ParseResponse>();

                    return !((int)getResp.StatusCode == 209);
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
                .WithHeader("Content-Type", "application/json")
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
            if (string.IsNullOrWhiteSpace(className)) throw new ArgumentNullException(nameof(className));

            try
            {
                var createUrl = BaseUrl + string.Format(ParseUrls.Class, className);
                var getResp = await createUrl
                    .SetQueryParams(new
                    {
                        where = query
                    })
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .GetAsync(token)
                    .ReceiveString();
                var mDeserializeObject =  JsonConvert.DeserializeObject<Results<T>>(getResp);
                return mDeserializeObject.results;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        internal class ParseResponse
        {
            public string Content { get; set; }
            public HttpStatusCode StatusCode { get; set; }
        }

        internal class Results<T>
        {
            public List<T> results { get; set; }
        }
    }
}
