using NUnit.Framework;
using SauceDemoAutomationUI.Pages;
using SauceDemoAutomationUI.Utilities;
using Allure.NUnit.Attributes;
using NUnit.Allure.Core;
using Allure.Net.Commons;

namespace SauceDemoAutomationUI.Tests
{
    [TestFixture]
    [Allure.NUnit.AllureNUnit]
    public class CheckoutTests : BaseTest
    {
        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.blocker)]
        [AllureOwner("Automation")]
        public void CompleteCheckout_ShouldShowThankYouMessage()
        {
            var loginPage = new LoginPage(Driver);
            var (username, password) = UserProvider.GetStandardUser();
            loginPage.Login(username, password);

            var inventoryPage = new InventoryPage(Driver);
            inventoryPage.AddItemToCart("Sauce Labs Backpack");
            inventoryPage.GoToCart();

            var cartPage = new CartPage(Driver);
            cartPage.ClickCheckout();

            var checkoutPage = new CheckoutPage(Driver);
            checkoutPage.FillCheckoutForm(TestDataProvider.GetFirstName(), TestDataProvider.GetLastName(), TestDataProvider.GetPostalCode());

            var overviewPage = new CheckoutOverviewPage(Driver);
            overviewPage.ClickFinish();

            var completePage = new CheckoutCompletePage(Driver);
            var message = completePage.GetThankYouMessage();

            Assert.AreEqual("Thank you for your order!", message);
        }
    }
}

