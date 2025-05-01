using NUnit.Framework;
using SauceDemoAutomationUI.Pages;
using SauceDemoAutomationUI.Utilities;
using Allure.NUnit.Attributes;
using NUnit.Allure.Core;
using Allure.Net.Commons;

namespace SauceDemoAutomationUI.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class CartTests : BaseTest
    {
        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void AddItemToCart_ShouldAppearInCart()
        {
            var loginPage = new LoginPage(Driver);
            var (username, password) = UserProvider.GetStandardUser();
            loginPage.Login(username, password);

            var inventoryPage = new InventoryPage(Driver);
            inventoryPage.AddItemToCart("Sauce Labs Backpack");
            inventoryPage.GoToCart();

            Assert.IsTrue(Driver.PageSource.Contains("Sauce Labs Backpack"));
        }
    }
}

