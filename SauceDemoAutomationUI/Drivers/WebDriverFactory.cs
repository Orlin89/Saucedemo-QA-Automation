using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using WebDriverManager;
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

            switch (browser.ToLower())
            {
                case "chrome":
                    var chromeVersion = Environment.GetEnvironmentVariable("CHROME_VERSION") ?? "latest";
                    new DriverManager().SetUpDriver(new ChromeConfig(), chromeVersion);
                    var chromeOptions = new ChromeOptions();
                    if (isCi)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                        chromeOptions.AddArgument("--disable-gpu");
                    }
                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    var firefoxVersion = Environment.GetEnvironmentVariable("FIREFOX_VERSION") ?? "latest";
                    new DriverManager().SetUpDriver(new FirefoxConfig(), firefoxVersion);
                    var firefoxOptions = new FirefoxOptions();
                    if (isCi)
                    {
                        firefoxOptions.AddArgument("--headless");
                    }
                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    var edgeVersion = Environment.GetEnvironmentVariable("EDGE_VERSION") ?? "latest";
                    new DriverManager().SetUpDriver(new EdgeConfig(), edgeVersion);
                    var edgeOptions = new EdgeOptions();
                    if (isCi)
                    {
                        edgeOptions.AddArgument("headless");
                        edgeOptions.AddArgument("disable-gpu");
                    }
                    driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new ArgumentException($"Browser not supported: {browser}");
            }

            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
