using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using parse.portable.net.Rest.Models;
using Parse.Api.Tests;
using Xunit;

namespace parse.portable.net.tests
{
    public class UserTests : IDisposable
    {
        private HttpTest _httpTest;
        private const string ValidUserId1 = "";
        private const string FakeUuid = "0000000000000000";
        private const string ClientID = "98743578e202eb43740849091ff8d0ea";
        private const string ParseEndPoint = "https://auto-2214.nodechef.com/parse";
        public UserTests()
        {
            _httpTest = new HttpTest();
        }

        [Fact]
        public async Task UserSignUp()
        {
            var parseObject = new ParseUser
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ObjectId = "dafdafd23"
            };
            var response = JsonConvert.SerializeObject(parseObject);

            _httpTest.RespondWithJson(response);

            var fakeUser = GetFakeUser();

            var parseClient = new ParseClient(ClientID, ParseEndPoint);

            var result = await parseClient.SignUp(fakeUser.Username, fakeUser.Password, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task TestCloudFunction()
        {
            string SMSExceptionString = $"SMS Cloud function test";
            string[] keys = { "uuid", "exception", "type" };

            object[] objects = { "ee9798fdb87da590", SMSExceptionString, 7 };

            var smsDictionary = new Dictionary<string, object>();
            for (int index = 0; index < keys.Length; index++)
            {
                smsDictionary.Add(keys[index], objects[index]);
            }
            var parseClient = new ParseClient(ClientID, ParseEndPoint);

            await LoginAsync();
           
            var functionResult = await parseClient.CallFunctionAsync<IDictionary<string, object>>("sms", smsDictionary, CancellationToken.None);
        }

        [Fact]
        public async Task LoginAsync()
        {
            var parseClient = new ParseClient(ClientID, ParseEndPoint);

            var result = await parseClient.LoginAsync("cprice70", "test4444", CancellationToken.None);

            Assert.True(!string.IsNullOrWhiteSpace(result?.SessionToken));
        }

        private static MyUser GetFakeUser()
        {
            var rand = new Random().Next();
            return new MyUser
            {
                Username = "testuser" + rand,
                Password = "pass" + rand,
                Email = "email" + rand + "@gmail.com",
            };
        }

        public void Dispose()
        {
            _httpTest.Dispose();
        }
    }
}
