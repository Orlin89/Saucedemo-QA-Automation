using OpenQA.Selenium;

namespace SauceDemoAutomationUI.Pages
{
    public class CheckoutCompletePage
    {
        private readonly IWebDriver _driver;
        private IWebElement ThankYouMessage => _driver.FindElement(By.ClassName("complete-header"));

        public CheckoutCompletePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string GetThankYouMessage()
        {
            return ThankYouMessage.Text;
        }
    }
}
