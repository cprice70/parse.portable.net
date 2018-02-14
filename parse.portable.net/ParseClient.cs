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
using Parse.Api;
using Parse.Api.Models;

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
                   .GetJsonAsync<ParseUser>(cancellationToken: cancellationToken);
                   

               return getResp;
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               return null;
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
        /// Creates a new ParseObject
        /// </summary>
        /// <param name="obj">The object to be created on the server</param>
        /// <param name="className"></param>
        /// <param name="token"></param>
        /// <returns>A fully populated ParseObject, including ObjectId</returns>
        public async Task<T> CreateObjectAsync<T>(string className, object obj, CancellationToken token) where T : ParseObject, new()
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentNullException("class_name");
            }

            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            try
            {
                var createUrl = BaseUrl + string.Format(ParseUrls.Class, className);
                var getResp = await createUrl
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .PostJsonAsync(obj, cancellationToken: token)
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
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentNullException("class_name");
            }

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
                var mDeserializeObject =  JsonConvert.DeserializeObject<Results>(getResp);
                return (IList<T>) mDeserializeObject.results;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Task<bool> SaveAsync()
        {
            return Task.FromResult(false);


            //url -X POST \
            //-H "X-Parse-Application-Id: ${APPLICATION_ID}" \
            //-H "X-Parse-REST-API-Key: ${REST_API_KEY}" \
            //-H "Content-Type: application/json" \
            //-d '{"score":1337,"playerName":"Sean Plott","cheatMode":false}' \
            //https://YOUR.PARSE-SERVER.HERE/parse/classes/GameScore
        }

        internal class ParseResponse
        {
            public string Content { get; set; }
            public HttpStatusCode StatusCode { get; set; }
        }

        internal class Results
        {
            public List<ParseObject> results { get; set; }
        }
    }
}
