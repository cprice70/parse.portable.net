using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using parse.portable.net.Rest;
using Parse.Api;
using Parse.Api.Models;

namespace parse.portable.net
{
    public class ParseClient
    {
        private readonly Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        private string AddId { get; }
        private string BaseUrl { get; } 
        private string ClientKey { get; }
        public ParseClient(string appId, string baseUrl, string clientKey = "")
        {
            AddId = appId;
            BaseUrl = baseUrl;
            ClientKey = clientKey;
            
            _settings = new Lazy<Newtonsoft.Json.JsonSerializerSettings>(() =>
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                //UpdateJsonSerializerSettings(settings);
                return settings;
            });
        }

        protected virtual void UpdateJsonSerializerSettings(ParseClient instance, Newtonsoft.Json.JsonSerializerSettings settings) { }
        protected virtual void PrepareRequest(HttpClient client, HttpRequestMessage request, string url) { }
        protected virtual void PrepareRequest(HttpClient client, HttpRequestMessage request, System.Text.StringBuilder urlBuilder) { }
        protected virtual void ProcessResponse(HttpClient client, HttpResponseMessage response) { }


        public async Task<bool> SignUp(string new_username, string new_password, CancellationToken token)
        {
            try
            {
                var signUpUrl = BaseUrl + ParseUrls.User;
                var getResp = await signUpUrl
                    .WithHeader(ParseHeaders.AppId, AddId)
                    .WithHeader("X-Parse-Revocable-Session", 1)
                    .WithHeader("Content-Type", "application/json")
                    .PostUrlEncodedAsync(new { 
                        username = new_username, 
                        password = new_password
                    }, token);
            
                return getResp.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
        }
        
        public async Task<ParseUserRequest> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            ParseUserRequest user = new ParseUserRequest();
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/parse/login");
            //urlBuilder_.Replace("{user_id}", System.Uri.EscapeDataString(ConvertToString(username, System.Globalization.CultureInfo.InvariantCulture)));

            var client = new HttpClient();
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    // --data - urlencode'username=cooldude6' \
                    //--data - urlencode 'password=p_n7!-e8'
                    PrepareRequest(client, request, urlBuilder);
                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client, request, url);
                    request.Content = new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("username", username),
                            new KeyValuePair<string, string>("password", password)
                        });
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers = Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        foreach (var item in response.Content.Headers)
                            headers[item.Key] = item.Value;

                        ProcessResponse(client, response);

                        var status = ((int)response.StatusCode).ToString();
                        if (status == "200")
                        {
                            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            try
                            {
                                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ParseUserRequest>(responseData, _settings?.Value);
                                return result;
                            }
                            catch (Exception exception)
                            {
                                throw new SwaggerException("Could not deserialize the response body.", status, responseData, headers, exception);
                            }
                        }
                        else
                        if (status != "200" && status != "204")
                        {
                            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new SwaggerException($"The HTTP status code of the response was not expected ({Convert.ToInt32(response.StatusCode)}).", status, responseData, headers, null);
                        }

                        return default(ParseUserRequest);
                    }
                    finally
                    {
                        response?.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                client.Dispose();
            }
        }

    }
}
