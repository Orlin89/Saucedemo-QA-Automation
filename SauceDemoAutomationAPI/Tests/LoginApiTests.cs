using NUnit.Framework;
using SauceDemoAutomationAPI.Clients;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using Allure.Net.Commons;

namespace SauceDemoAutomationAPI.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class LoginApiTests
    {
        private LoginApiClient _loginApiClient;

        [SetUp]
        public void SetUp()
        {
            _loginApiClient = new LoginApiClient();
        }

        [Test]
        [AllureTag("API")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Automation")]
        public void Login_WithValidUser_ShouldReturnSuccess()
        {
            var response = _loginApiClient.Login("standard_user", "secret_sauce");
            Assert.IsTrue(response.IsSuccessful);
        }

        [Test]
        [AllureTag("API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void Login_WithInvalidUser_ShouldReturnUnauthorized()
        {
            var response = _loginApiClient.Login("invalid_user", "wrong_password");
            Assert.AreEqual(401, (int)response.StatusCode);
        }
    }
}

