using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using SauceDemoAutomationUI.Pages;
using SauceDemoAutomationUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoAutomationUI.Tests
{
    [TestFixture("chrome")]
    [TestFixture("firefox")]
    [TestFixture("edge")]
    [AllureNUnit]
    [Parallelizable(ParallelScope.Self)]
    public class InventoryTests : BaseTests
    {
        public InventoryTests(string browser) : base(browser) { }

        [SetUp]
        public void SetUpInventory()
        {           
            var (username, password) = UserProvider.GetStandardUser();
            Login(username, password);
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void TestInventoryDisplay()
        {          
            var inventoryPage = new InventoryPage(driver);

            Assert.That(inventoryPage.IsINventoryDisplayed(), Is.True);
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void TestAddToCartByIndex()
        {           
            var inventoryPage = new InventoryPage(driver);

            inventoryPage.AddToCartByIndex(2);
            inventoryPage.ClickCartLink();

            var cartPage = new CartPage(driver);
            Assert.That(cartPage.IsCartItemDisplayed(), Is.True);
        }

        [Test]
        [AllureTag("UI")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Automation")]
        public void AddToCartByName()
        {           
            var inventoryPage = new InventoryPage(driver);

            inventoryPage.AddToCartByName("Sauce Labs Backpack");
            inventoryPage.ClickCartLink();

            var cartPage = new CartPage(driver);
            Assert.That(cartPage.IsCartItemDisplayed(), Is.True);
        }

        [Test]
        public void TestPageTitle()
        {           
            var inventoryPage = new InventoryPage(driver);

            Assert.That(inventoryPage.IsPageLoaded, Is.True);
        }
    }
}
