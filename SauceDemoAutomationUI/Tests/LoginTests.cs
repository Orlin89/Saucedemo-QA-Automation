using NUnit.Framework;
using SauceDemoAutomationUI.Pages;
using SauceDemoAutomationUI.Utilities;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using Allure.Net.Commons;

namespace SauceDemoAutomationUI.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class LoginTests : BaseTest
    {
        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Automation")]
        public void StandardUserLogin_ShouldSucceed()
        {
            var loginPage = new LoginPage(Driver);
            var (username, password) = UserProvider.GetStandardUser();
            loginPage.Login(username, password);

            Assert.IsTrue(Driver.Url.Contains("inventory.html"));
        }
    }
}

