using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
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
                    var chromeOptions = new ChromeOptions();
                    if (isCi)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                        chromeOptions.AddArgument("--disable-gpu");
                        driver = new ChromeDriver("/usr/local/bin", chromeOptions);
                    }
                    else
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new WebDriverManager.DriverConfigs.Impl.ChromeConfig());
                        driver = new ChromeDriver(chromeOptions);
                    }
                    break;

                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    if (isCi)
                    {
                        firefoxOptions.AddArgument("--headless");
                        driver = new FirefoxDriver("/usr/local/bin", firefoxOptions); 
                    }
                    else
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new WebDriverManager.DriverConfigs.Impl.FirefoxConfig());
                        driver = new FirefoxDriver(firefoxOptions);
                    }
                    break;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    if (isCi)
                    {
                        edgeOptions.AddArgument("headless");
                        edgeOptions.AddArgument("disable-gpu");
                        driver = new EdgeDriver("/usr/local/bin", edgeOptions);
                    }
                    else
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new WebDriverManager.DriverConfigs.Impl.EdgeConfig());
                        driver = new EdgeDriver(edgeOptions);
                    }
                    break;

                default:
                    throw new ArgumentException($"Browser not supported: {browser}");
            }

            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
