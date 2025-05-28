using NUnit.Framework;
using OpenQA.Selenium;
using SauceDemoAutomationUI.Drivers;
using SauceDemoAutomationUI.Pages;

namespace SauceDemoAutomationUI.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class BaseTests
    {
        protected IWebDriver driver;
        private readonly string _browser;

        public BaseTests(string browser)
        {
            _browser = browser;
        }

        [SetUp]
        public void SetUp()
        {
            driver = WebDriverFactory.CreateDriver(_browser);
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }

        public void Login(string username, string password)
        {
            var loginPage = new LoginPage(driver);
            loginPage.InputUsername(username);
            loginPage.InputPassword(password);
            loginPage.ClickLoginButton();
        }
    }
}
