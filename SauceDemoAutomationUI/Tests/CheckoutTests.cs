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
    public class CheckoutTests : BaseTests
    {
        public CheckoutTests(string browser) : base(browser) { }

        [SetUp]
        public void SetUpCheckout()
        {
            var (username, password) = UserProvider.GetStandardUser();
            Login(username, password);
            var inventoryPage = new InventoryPage(driver);
            inventoryPage.AddToCartByIndex(3);
            inventoryPage.ClickCartLink();
            var cartPage = new CartPage(driver);
            cartPage.ClickCheckout();
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.blocker)]
        [AllureOwner("Automation")]
        public void TestCheckoutPageLoaded()
        {
            var checkoutPage = new CheckoutPage(driver);
            Assert.That(checkoutPage.IsPageLoaded, Is.True);
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.blocker)]
        [AllureOwner("Automation")]
        public void TestContinueToNextStep()
        {
            var checkoutPage = new CheckoutPage(driver);
            checkoutPage.EnterFirstName("SomeName");
            checkoutPage.EnterLastName("LastName");
            checkoutPage.EnterPostalCode("1000");
            checkoutPage.ClickContinue();

            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/checkout-step-two.html"));
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.blocker)]
        [AllureOwner("Automation")]
        public void TestCompleteOrder()
        {
            var checkoutPage = new CheckoutPage(driver);
            checkoutPage.EnterFirstName("SomeName");
            checkoutPage.EnterLastName("LastName");
            checkoutPage.EnterPostalCode("1000");
            checkoutPage.ClickContinue();
            checkoutPage.ClickFinish();

            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/checkout-complete.html"));
            Assert.That(checkoutPage.IsCheckoutComplete(), Is.True);
        }
    }
}

