using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using WebDriverManager.DriverConfigs.Impl;
using System;

namespace SauceDemoAutomationUI.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateDriver(string browser)
        {
            IWebDriver driver;
            bool isCi = Environment.GetEnvironmentVariable("CI") == "true";
            string driverPath = "/usr/local/bin";

            switch (browser.ToLower())
            {
                case "chrome":
                    if (!isCi)
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                    }
                    var chromeOptions = new ChromeOptions();

                    if (isCi)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                        chromeOptions.AddArgument("--disable-gpu");
                        driver = new ChromeDriver(driverPath, chromeOptions);
                    }
                    else
                    {
                        chromeOptions.AddArgument("--incognito");
                        chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                        chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                        driver = new ChromeDriver(chromeOptions);
                    }
                    break;

                case "firefox":
                    if (!isCi)
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                    }
                    var firefoxOptions = new FirefoxOptions();

                    if (isCi)
                    {
                        firefoxOptions.AddArgument("--headless");
                        driver = new FirefoxDriver(driverPath, firefoxOptions);
                    }
                    else
                    {
                        driver = new FirefoxDriver(firefoxOptions);
                    }
                    break;

                case "edge":
                    if (!isCi)
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                    }
                    var edgeOptions = new EdgeOptions();

                    if (isCi)
                    {
                        edgeOptions.AddArgument("headless");
                        edgeOptions.AddArgument("disable-gpu");
                        driver = new EdgeDriver(driverPath, edgeOptions);
                    }
                    else
                    {
                        driver = new EdgeDriver(edgeOptions);
                    }
                    break;

                default:
                    throw new ArgumentException($"Browser not supported: {browser}");
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return driver;
        }
    }
}
