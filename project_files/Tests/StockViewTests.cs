using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core;
using System.Drawing.Text;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Data.SQLite;

namespace Tests
{
    [TestClass]
    public sealed class StockViewTests
    {
        private String applicationPath = Path.GetFullPath(@"..\..\..\..\PointOfSale\bin\Debug\net9.0-windows\PointOfSale.exe");
        private Application application;
        private Window mainWindow;
        private ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

        [TestInitialize]
        public void Setup()
        {
            TestSetupAndCleanup.InitializeTestDatabase();
            TestSetupAndCleanup.ProtectUserReceipts();
            TestSetupAndCleanup.ProtectUserConfiguration();

            application = Application.Launch(applicationPath);
            mainWindow = application.GetMainWindow(new UIA3Automation());
            TestSetupAndCleanup.Login(mainWindow, cf);
        }

        [TestCleanup]
        public void Cleanup()
        {
            application.Close();
            TestSetupAndCleanup.RestoreReceiptDirectory();
            TestSetupAndCleanup.RestoreUserConfiguration();
            TestSetupAndCleanup.RemoveTestDatabase();
        }
        [TestMethod]
        public void TestInitialState()
        {
            var stockTab = mainWindow.FindFirstDescendant(cf.ByName("Lager"));

            stockTab.Click();

            var firstProduct = mainWindow.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            Assert.IsTrue(firstProduct.FindFirstDescendant(cf.ByName("100")) is not null);
        }

        [TestMethod]
        public void TestRemoveFromStock()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Marabou Mjölkchoklad 100 g"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));
            var stockTab = mainWindow.FindFirstDescendant(cf.ByName("Lager"));

            for (int i = 0; i < 10; i++)
            {
                addProductButton.Click();
            }

            checkoutButton.Click();

            stockTab.Click();

            var updateStockButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("UpdateStockButton"));

            updateStockButton.Click();

            var firstProduct = mainWindow.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            Assert.IsTrue(firstProduct.FindFirstDescendant(cf.ByName("90")) is not null);
        }

        [TestMethod]
        public void TestSortStockView()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));
            var stockTab = mainWindow.FindFirstDescendant(cf.ByName("Lager"));

            for (int i = 0; i < 4; i++)
            {
                addProductButton.Click();
            }

            checkoutButton.Click();

            stockTab.Click();

            var updateStockButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("UpdateStockButton"));
            updateStockButton.Click();

            var stockView = mainWindow.FindFirstDescendant(cf.ByAutomationId("StockView"));
            var firstProduct = stockView.FindFirstDescendant(cf.ByClassName("DataGridRow"));
            Assert.IsTrue(firstProduct.FindFirstDescendant(cf.ByName("Marabou Mjölkchoklad 100 g")) is not null);

            //Sorts products by name (ascending)
            var sortByName = stockView.FindFirstDescendant(cf.ByName("Produktnamn"));
            sortByName.Click();
            firstProduct = stockView.FindFirstDescendant(cf.ByClassName("DataGridRow"));
            Assert.IsTrue(firstProduct.FindFirstDescendant(cf.ByName("Aftonbladet (dagens)")) is not null);

            //Sorts products by name (descending)
            sortByName.Click();
            firstProduct = stockView.FindFirstDescendant(cf.ByClassName("DataGridRow"));
            Assert.IsTrue(firstProduct.FindFirstDescendant(cf.ByName("Varm toast (ost & skinka)")) is not null);

            //Sorts products by stock (ascending)
            var sortByStock = stockView.FindFirstDescendant(cf.ByName("Lager"));
            sortByStock.Click();
            firstProduct = stockView.FindFirstDescendant(cf.ByClassName("DataGridRow"));
            Assert.IsTrue(firstProduct.FindFirstDescendant(cf.ByName("Kexchoklad")) is not null);
            Assert.IsTrue(firstProduct.FindFirstDescendant(cf.ByName("96")) is not null);
        }
    }
}
