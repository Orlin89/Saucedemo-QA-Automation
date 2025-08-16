using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO.Compression;

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
                    driver = CreateEdgeDriver(isCi, driverPath);
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

        private static IWebDriver CreateEdgeDriver(bool isCi, string driverPath)
        {
            var edgeOptions = new EdgeOptions();

            if (isCi)
            {
                // Check if EdgeDriver exists in CI
                string edgeDriverPath = Path.Combine(driverPath, "msedgedriver");
                if (!File.Exists(edgeDriverPath))
                {
                    throw new NotSupportedException($"EdgeDriver not found at {edgeDriverPath}. Edge tests are not supported in CI environment due to Microsoft infrastructure changes.");
                }

                edgeOptions.AddArgument("--headless=new");
                edgeOptions.AddArgument("--no-sandbox");
                edgeOptions.AddArgument("--disable-dev-shm-usage");
                edgeOptions.AddArgument("--disable-gpu");
                edgeOptions.AddArgument("--window-size=1920,1080");
                edgeOptions.AddArgument("--disable-extensions");
                edgeOptions.AddArgument("--disable-web-security");

                return new EdgeDriver(driverPath, edgeOptions);
            }
            else
            {
                // For local development, try multiple approaches
                return TryCreateLocalEdgeDriver(edgeOptions);
            }
        }

        private static IWebDriver TryCreateLocalEdgeDriver(EdgeOptions edgeOptions)
        {
            // Get Edge version first
            string edgeVersion = GetEdgeVersion();
            Console.WriteLine($"Detected Edge version: {edgeVersion}");

            // Try method 1: Use Selenium Manager (Selenium 4.6+)
            try
            {
                Console.WriteLine("Attempting Edge with Selenium Manager...");
                // Selenium Manager should automatically download the correct driver
                return new EdgeDriver(edgeOptions);
            }
            catch (Exception ex1)
            {
                Console.WriteLine($"Selenium Manager failed: {ex1.Message}");
            }

            // Try method 2: Manual EdgeDriver download
            try
            {
                Console.WriteLine("Attempting manual EdgeDriver setup...");
                string driverPath = DownloadEdgeDriver(edgeVersion);
                if (!string.IsNullOrEmpty(driverPath))
                {
                    var service = EdgeDriverService.CreateDefaultService(Path.GetDirectoryName(driverPath));
                    return new EdgeDriver(service, edgeOptions);
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine($"Manual EdgeDriver setup failed: {ex2.Message}");
            }

            // Try method 3: WebDriverManager as last resort (might fail due to Microsoft changes)
            try
            {
                Console.WriteLine("Attempting WebDriverManager as last resort...");
                new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                return new EdgeDriver(edgeOptions);
            }
            catch (Exception ex3)
            {
                Console.WriteLine($"WebDriverManager failed: {ex3.Message}");
            }

            // If all methods fail, provide helpful error message
            throw new NotSupportedException(
                $"Unable to start Edge browser. This is likely due to version mismatch or Microsoft's infrastructure changes.\n" +
                $"Detected Edge version: {edgeVersion}\n" +
                $"Solutions:\n" +
                $"1. Update to Selenium 4.15+ which has improved Selenium Manager\n" +
                $"2. Manually download EdgeDriver from Microsoft Developer site\n" +
                $"3. Use Chrome or Firefox for testing (recommended)\n" +
                $"4. Consider using WebDriver BiDi protocol for future-proofing");
        }

        private static string GetEdgeVersion()
        {
            try
            {
                // Try multiple registry paths for Edge
                string[] registryPaths = {
                    @"SOFTWARE\Microsoft\Edge\BLBeacon",
                    @"SOFTWARE\WOW6432Node\Microsoft\Edge\BLBeacon",
                    @"SOFTWARE\Microsoft\EdgeUpdate\Clients\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}"
                };

                foreach (string path in registryPaths)
                {
                    try
                    {
                        var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);
                        if (key != null)
                        {
                            var version = key.GetValue("version")?.ToString();
                            if (!string.IsNullOrEmpty(version))
                            {
                                return version;
                            }
                        }
                    }
                    catch { }
                }

                // Fallback: try to get version from Edge executable
                string edgePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
                if (File.Exists(edgePath))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(edgePath);
                    return versionInfo.FileVersion;
                }

                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        private static string DownloadEdgeDriver(string edgeVersion)
        {
            try
            {
                Console.WriteLine($"Attempting to download EdgeDriver for version {edgeVersion}...");

                // Microsoft's new EdgeDriver distribution is through Microsoft Developer site
                // We'll try to download the correct version
                string driverDir = Path.Combine(Path.GetTempPath(), "EdgeDriver");
                Directory.CreateDirectory(driverDir);

                string driverPath = Path.Combine(driverDir, "msedgedriver.exe");

                // If already downloaded and cached, use it
                if (File.Exists(driverPath))
                {
                    Console.WriteLine($"Found cached EdgeDriver at {driverPath}");
                    return driverPath;
                }

                // Extract major version (e.g., 139 from 139.0.3405.102)
                string majorVersion = edgeVersion.Split('.')[0];

                // Try Microsoft's new EdgeDriver endpoints
                string[] downloadUrls = {
                    $"https://msedgedriver.azureedge.net/{edgeVersion}/edgedriver_win64.zip",
                    $"https://msedgedriver.azureedge.net/{majorVersion}.0.0.0/edgedriver_win64.zip",
                    // Fallback to GitHub releases (community maintained)
                    $"https://github.com/MicrosoftEdge/EdgeWebDriver/releases/download/{majorVersion}/edgedriver_win64.zip"
                };

                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);

                    foreach (string url in downloadUrls)
                    {
                        try
                        {
                            Console.WriteLine($"Trying download from: {url}");
                            var response = httpClient.GetAsync(url).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var zipBytes = response.Content.ReadAsByteArrayAsync().Result;
                                string zipPath = Path.Combine(driverDir, "edgedriver.zip");
                                File.WriteAllBytes(zipPath, zipBytes);

                                // Extract the zip file
                                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, driverDir, true);
                                File.Delete(zipPath);

                                if (File.Exists(driverPath))
                                {
                                    Console.WriteLine($"Successfully downloaded EdgeDriver to {driverPath}");
                                    return driverPath;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to download from {url}: {ex.Message}");
                        }
                    }
                }

                Console.WriteLine("All EdgeDriver download attempts failed");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EdgeDriver download failed: {ex.Message}");
                return null;
            }
        }
    }
}