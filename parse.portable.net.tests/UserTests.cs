using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Parse.Api.Models;
using Parse.Api.Tests;

namespace parse.portable.net.tests
{
    [TestFixture]
    public class UserTests
    {
        private const string ValidUserId1 = "";

        [Test]
        public async Task UserSignUp()
        {
            var fakeUser = GetFakeUser();
            
            var parseClient = new ParseClient("98743578e202eb43740849091ff8d0ea", "https://auto-2214.nodechef.com/parse/");

            var result = await parseClient.SignUp(fakeUser.Username, fakeUser.Password, CancellationToken.None);
            
            Assert.True(result);
        }
        
        [Test]
        public async Task LoginAsync()
        {
            var parseClient = new ParseClient("98743578e202eb43740849091ff8d0ea", "https://auto-2214.nodechef.com/parse/");

            var result = await parseClient.LoginAsync("cprice70", "test4444", CancellationToken.None);
            
            Assert.True(!string.IsNullOrWhiteSpace(result?.SessionToken));
        }

        #region helpers

        private static ParseUnitTestObj GetFakeObj()
        {
            return new ParseUnitTestObj
            {
                SomeByte = 1,
                SomeShort = 2,
                SomeInt = 3,
                SomeLong = 4,
                SomeNullableBool = null,
                SomeGeoPoint = new ParseGeoPoint(40, 40),
                SomeBytes = new byte[] {1, 2, 3},
                SomeDate = DateTime.UtcNow.AddDays(-10),
                SomeNullableDate = DateTime.UtcNow.AddDays(-30),
                SomePointer = new MyUser {ObjectId = ValidUserId1},
                SomeObject = new {Rando = true},
                SomeArray = new[] {1, 2, 3},
            };
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

        private static void AssertParseObjectEqual<T>(T obj1, T obj2) where T : ParseObject
        {
            if (obj1 == null && obj2 == null)
            {
                return;
            }

            Assert.IsNotNull(obj1);
            Assert.IsNotNull(obj2);

            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.PropertyType.IsClass)
                {
                    continue;
                }

                var prop1 = prop.GetValue(obj1, null);
                var prop2 = prop.GetValue(obj2, null);

                if (prop.PropertyType == typeof(DateTime))
                {
                    var diff = ((DateTime) prop1).Subtract((DateTime) prop2);
                    Assert.IsTrue(Math.Abs(diff.TotalMilliseconds) < 1);
                }
                else
                {
                    Assert.AreEqual(prop1, prop2);
                }
            }
        }

        #endregion
    }
}