using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.IO;

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
                        // Optimized Chrome options for CI
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                        chromeOptions.AddArgument("--disable-extensions");
                        chromeOptions.AddArgument("--disable-background-timer-throttling");
                        chromeOptions.AddArgument("--disable-backgrounding-occluded-windows");
                        chromeOptions.AddArgument("--disable-renderer-backgrounding");
                        chromeOptions.AddArgument("--disable-ipc-flooding-protection");
                        chromeOptions.AddArgument("--enable-automation");
                        chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                        chromeOptions.AddExcludedArgument("enable-automation");
                        chromeOptions.AddUserProfilePreference("useAutomationExtension", false);

                        var service = ChromeDriverService.CreateDefaultService(driverPath);
                        service.SuppressInitialDiagnosticInformation = true;
                        service.EnableVerboseLogging = false;

                        driver = new ChromeDriver(service, chromeOptions, TimeSpan.FromMinutes(3));
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
                        firefoxOptions.AddArgument("--width=1920");
                        firefoxOptions.AddArgument("--height=1080");
                        firefoxOptions.SetPreference("dom.webdriver.enabled", false);
                        firefoxOptions.SetPreference("useAutomationExtension", false);
                        firefoxOptions.SetPreference("dom.webnotifications.enabled", false);
                        firefoxOptions.SetPreference("media.volume_scale", "0.0");
                        firefoxOptions.SetPreference("browser.startup.homepage", "about:blank");
                        firefoxOptions.SetPreference("startup.homepage_welcome_url", "about:blank");
                        firefoxOptions.SetPreference("startup.homepage_welcome_url.additional", "about:blank");

                        var service = FirefoxDriverService.CreateDefaultService(driverPath);
                        service.SuppressInitialDiagnosticInformation = true;
                      

                        driver = new FirefoxDriver(service, firefoxOptions, TimeSpan.FromMinutes(5));
                    }
                    else
                    {
                        driver = new FirefoxDriver(firefoxOptions);
                    }
                    break;

                case "edge":
                    // Check if EdgeDriver exists before trying to use it
                    string edgeDriverPath = Path.Combine(driverPath, "msedgedriver");
                    if (isCi && !File.Exists(edgeDriverPath))
                    {
                        throw new NotSupportedException($"EdgeDriver not found at {edgeDriverPath}. Edge tests are currently not supported in CI environment due to driver installation issues.");
                    }

                    if (!isCi)
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                    }
                    var edgeOptions = new EdgeOptions();

                    if (isCi)
                    {
                        // Fixed Edge options for CI
                        edgeOptions.AddArgument("--headless=new");
                        edgeOptions.AddArgument("--no-sandbox");
                        edgeOptions.AddArgument("--disable-dev-shm-usage");
                        edgeOptions.AddArgument("--disable-gpu");
                        edgeOptions.AddArgument("--window-size=1920,1080");
                        edgeOptions.AddArgument("--disable-extensions");
                        edgeOptions.AddArgument("--disable-web-security");

                        var service = EdgeDriverService.CreateDefaultService(driverPath);
                        service.SuppressInitialDiagnosticInformation = true;

                        driver = new EdgeDriver(service, edgeOptions, TimeSpan.FromMinutes(3));
                    }
                    else
                    {
                        driver = new EdgeDriver(edgeOptions);
                    }
                    break;

                default:
                    throw new ArgumentException($"Browser not supported: {browser}");
            }

            // Enhanced driver configuration
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);

            return driver;
        }
    }
}