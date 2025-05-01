using NUnit.Framework;
using OpenQA.Selenium;
using SauceDemoAutomationUI.Drivers;

namespace SauceDemoAutomationUI.Tests
{
    public class BaseTest
    {
        protected IWebDriver Driver;
        private string _browser;

        [SetUp]
        public void SetUp()
        {
            _browser = TestContext.Parameters.Get("browser", "chrome");
            Driver = WebDriverFactory.CreateDriver(_browser);
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [TearDown]
        public void TearDown()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
            }
        }
    }
}

