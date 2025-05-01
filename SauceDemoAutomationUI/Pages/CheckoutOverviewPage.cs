using OpenQA.Selenium;

namespace SauceDemoAutomationUI.Pages
{
    public class CheckoutOverviewPage
    {
        private readonly IWebDriver _driver;
        private IWebElement FinishButton => _driver.FindElement(By.Id("finish"));

        public CheckoutOverviewPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ClickFinish()
        {
            FinishButton.Click();
        }
    }
}

