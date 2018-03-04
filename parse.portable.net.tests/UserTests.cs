using System;
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

        public UserTests()
        {
            _httpTest = new HttpTest();
        }

        [Fact]
        public async Task UserSignUp()
        {
            var parseObject = new ParseUser();
            parseObject.CreatedAt = DateTime.Now;
            parseObject.UpdatedAt = DateTime.Now;
            parseObject.ObjectId = "dafdafd23";
            var response = JsonConvert.SerializeObject(parseObject);

            _httpTest.RespondWithJson(response);

            var fakeUser = GetFakeUser();

            var parseClient = new ParseClient("98743578e202eb43740849091ff8d0ea", "https://auto-2214.nodechef.com/parse/");

            var result = await parseClient.SignUp(fakeUser.Username, fakeUser.Password, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task LoginAsync()
        {
            var parseClient = new ParseClient("98743578e202eb43740849091ff8d0ea", "https://auto-2214.nodechef.com/parse/");

            var result = await parseClient.LoginAsync("cprice70", "test4444", CancellationToken.None);

            Assert.True(!string.IsNullOrWhiteSpace(result.SessionToken));
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
