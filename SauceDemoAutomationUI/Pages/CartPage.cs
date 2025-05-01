using OpenQA.Selenium;

namespace SauceDemoAutomationUI.Pages
{
    public class CartPage
    {
        private readonly IWebDriver _driver;

        private IWebElement CheckoutButton => _driver.FindElement(By.Id("checkout"));

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ClickCheckout()
        {
            CheckoutButton.Click();
        }
    }
}


