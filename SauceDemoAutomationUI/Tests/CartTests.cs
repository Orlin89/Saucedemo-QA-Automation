using NUnit.Framework;
using SauceDemoAutomationUI.Pages;
using SauceDemoAutomationUI.Utilities;
using Allure.NUnit.Attributes;
using NUnit.Allure.Core;
using Allure.Net.Commons;

namespace SauceDemoAutomationUI.Tests
{
    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [AllureNUnit]
    [Parallelizable(ParallelScope.Self)]
    public class CartTests : BaseTests
    {
        public CartTests(string browser) : base(browser) { }

        [SetUp]
        public void SetUpCartTests()
        {
            var (username, password) = UserProvider.GetStandardUser();
            Login(username, password);
            var inventoryPage = new InventoryPage(driver);
            inventoryPage.AddToCartByIndex(2);
            inventoryPage.ClickCartLink();
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void TestCartItemDisplayed()
        {
            var cartPage = new CartPage(driver);
            Assert.That(cartPage.IsCartItemDisplayed(), Is.True);
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void TestClickCheckout()
        {
            var cartPage = new CartPage(driver);
            cartPage.ClickCheckout();
            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/checkout-step-one.html"));
        }
    }
}

