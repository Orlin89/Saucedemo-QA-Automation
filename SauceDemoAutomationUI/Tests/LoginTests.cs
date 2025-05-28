using NUnit.Framework;
using SauceDemoAutomationUI.Pages;
using SauceDemoAutomationUI.Utilities;
using Allure.NUnit.Attributes;
using NUnit.Allure.Core;
using Allure.Net.Commons;

namespace SauceDemoAutomationUI.Tests
{
    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [AllureNUnit]
    [Parallelizable(ParallelScope.Self)]
    public class LoginTests : BaseTests
    {
        public LoginTests(string browser) : base(browser) { }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Automation")]
        public void TestLoginWithValidCredentials()
        {
            var (username, password) = UserProvider.GetStandardUser();
            Login(username, password);

            var inventoryPage = new InventoryPage(driver);

            Assert.That(inventoryPage.IsPageLoaded(), Is.True);
        }      

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Automation")]
        public void TestLoginWithLockedOutUser()
        {
            var (username, password) = UserProvider.GetLockedOutUser();
            Login(username, password); ;

            var loginPage = new LoginPage(driver);

            Assert.That(loginPage.GetErrorMessage(), Is.EqualTo("Epic sadface: Sorry, this user has been locked out."));
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Automation")]
        public void TestLoginWithInvalidCredentials()
        {
            Login("InvalidUser", "InvalidPassword");

            var loginPage = new LoginPage(driver);

            Assert.That(loginPage.GetErrorMessage(), Is.EqualTo("Epic sadface: Username and password do not match any user in this service"));
        }
    }
}

