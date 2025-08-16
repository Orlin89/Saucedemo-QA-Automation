using NUnit.Framework;
using OpenQA.Selenium;
using SauceDemoAutomationUI.Drivers;
using SauceDemoAutomationUI.Pages;
using System;

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
            try
            {
                Console.WriteLine($"Setting up test with browser: {_browser}");
                driver = WebDriverFactory.CreateDriver(_browser);

                Console.WriteLine("Navigating to SauceDemo...");
                driver.Navigate().GoToUrl("https://www.saucedemo.com/");

                // Wait for page to load completely
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Setup completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Setup failed for browser {_browser}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                driver?.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during teardown: {ex.Message}");
            }
            finally
            {
                driver?.Dispose();
                driver = null;
            }
        }

        public void Login(string username, string password)
        {
            var loginPage = new LoginPage(driver);
            loginPage.InputUsername(username);
            loginPage.InputPassword(password);
            loginPage.ClickLoginButton();

            // Add small wait after login
            System.Threading.Thread.Sleep(1000);
        }
    }
}