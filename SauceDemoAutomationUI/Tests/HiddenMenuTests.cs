using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using SauceDemoAutomationUI.Pages;
using SauceDemoAutomationUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoAutomationUI.Tests
{
    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [AllureNUnit]
    [Parallelizable(ParallelScope.Self)]
    public class HiddenMenuTests : BaseTests
    {
        public HiddenMenuTests(string browser) : base(browser) { }       

            [SetUp]
        public void SetUpHiddenMenu()
        {
            var (username, password) = UserProvider.GetStandardUser();
            Login(username, password);
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void TestOpenMenu()
        {
            var hiddenMenu = new HiddenMenuPage(driver);
            hiddenMenu.ClickMenuButton();
            Assert.That(hiddenMenu.IsMenuOpen(), Is.True);
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void TestLogout()
        {
            var hiddenMenu = new HiddenMenuPage(driver);
            hiddenMenu.ClickMenuButton();
            hiddenMenu.ClickLogoutButton();

            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/"));
        }
    }
}
