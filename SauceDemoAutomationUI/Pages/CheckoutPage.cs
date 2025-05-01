using OpenQA.Selenium;

namespace SauceDemoAutomationUI.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;

        private IWebElement FirstNameInput => _driver.FindElement(By.Id("first-name"));
        private IWebElement LastNameInput => _driver.FindElement(By.Id("last-name"));
        private IWebElement PostalCodeInput => _driver.FindElement(By.Id("postal-code"));
        private IWebElement ContinueButton => _driver.FindElement(By.Id("continue"));

        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void FillCheckoutForm(string firstName, string lastName, string postalCode)
        {
            FirstNameInput.SendKeys(firstName);
            LastNameInput.SendKeys(lastName);
            PostalCodeInput.SendKeys(postalCode);
            ContinueButton.Click();
        }
    }
}

