using OpenQA.Selenium;

namespace SauceDemoAutomationUI.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver _driver;
        private IWebElement CartButton => _driver.FindElement(By.Id("shopping_cart_container"));

        public InventoryPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void AddItemToCart(string itemName)
        {
            _driver.FindElement(By.XPath($"//div[text()='{itemName}']/ancestor::div[@class='inventory_item']//button")).Click();
        }

        public void GoToCart()
        {
            CartButton.Click();
        }
    }
}

