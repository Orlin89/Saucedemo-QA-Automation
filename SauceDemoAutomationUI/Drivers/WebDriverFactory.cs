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
                        // Fixed Chrome options for CI - REMOVED --disable-javascript
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--disable-web-security");
                        chromeOptions.AddArgument("--disable-features=VizDisplayCompositor");
                        chromeOptions.AddArgument("--remote-debugging-port=9222");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                        chromeOptions.AddArgument("--start-maximized");
                        chromeOptions.AddArgument("--disable-extensions");
                        chromeOptions.AddArgument("--disable-plugins");
                        chromeOptions.AddArgument("--disable-images");
                        chromeOptions.AddArgument("--disable-background-timer-throttling");
                        chromeOptions.AddArgument("--disable-backgrounding-occluded-windows");
                        chromeOptions.AddArgument("--disable-renderer-backgrounding");
                        chromeOptions.AddArgument("--disable-ipc-flooding-protection");
                        // Add these for better stability
                        chromeOptions.AddArgument("--disable-background-networking");
                        chromeOptions.AddArgument("--disable-default-apps");
                        chromeOptions.AddArgument("--disable-sync");

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
                        firefoxOptions.AddArgument("--width=1920");
                        firefoxOptions.AddArgument("--height=1080");
                        firefoxOptions.SetPreference("dom.webdriver.enabled", false);
                        firefoxOptions.SetPreference("useAutomationExtension", false);
                        firefoxOptions.SetPreference("dom.webnotifications.enabled", false);
                        firefoxOptions.SetPreference("media.volume_scale", "0.0");
                        // Add preferences for better stability
                        firefoxOptions.SetPreference("browser.tabs.remote.autostart", false);
                        firefoxOptions.SetPreference("browser.tabs.remote.autostart.2", false);

                        driver = new FirefoxDriver(driverPath, firefoxOptions);
                    }
                    else
                    {
                        driver = new FirefoxDriver(firefoxOptions);
                    }
                    break;

                case "edge":
                    // Simplified Edge handling - fail fast if not available
                    string edgeDriverPath = Path.Combine(driverPath, "msedgedriver");
                    if (isCi && !File.Exists(edgeDriverPath))
                    {
                        throw new NotSupportedException($"EdgeDriver not found at {edgeDriverPath}. Skipping Edge tests in CI.");
                    }

                    if (!isCi)
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                    }
                    var edgeOptions = new EdgeOptions();

                    if (isCi)
                    {
                        edgeOptions.AddArgument("--headless=new");
                        edgeOptions.AddArgument("--no-sandbox");
                        edgeOptions.AddArgument("--disable-dev-shm-usage");
                        edgeOptions.AddArgument("--disable-gpu");
                        edgeOptions.AddArgument("--window-size=1920,1080");
                        edgeOptions.AddArgument("--disable-extensions");
                        edgeOptions.AddArgument("--disable-web-security");

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

            // Enhanced driver configuration with longer timeouts for CI
            driver.Manage().Window.Maximize();
            var implicitWait = isCi ? TimeSpan.FromSeconds(15) : TimeSpan.FromSeconds(10);
            var pageLoadTimeout = isCi ? TimeSpan.FromSeconds(60) : TimeSpan.FromSeconds(30);

            driver.Manage().Timeouts().ImplicitWait = implicitWait;
            driver.Manage().Timeouts().PageLoad = pageLoadTimeout;
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);

            return driver;
        }
    }
}