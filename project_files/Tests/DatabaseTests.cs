using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using System.Data.SQLite;

namespace Tests
{
    [TestClass]
    public sealed class DatabaseTests
    {
        public static string ReadData(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(TestSetupAndCleanup.connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    return result == null ? "" : result.ToString();
                }
            }
        }

        private String applicationPath = Path.GetFullPath(@"..\..\..\..\PointOfSale\bin\Debug\net9.0-windows\PointOfSale.exe");
        private Application application;
        private Window mainWindow;
        private ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

        [TestInitialize]
        public void Setup()
        {
            TestSetupAndCleanup.InitializeTestDatabase();
            TestSetupAndCleanup.ProtectUserReceipts();

            application = Application.Launch(applicationPath);
            mainWindow = application.GetMainWindow(new UIA3Automation());
        }

        [TestCleanup]
        public void Cleanup()
        {
            application.Close();
            TestSetupAndCleanup.RemoveTestDatabase();
            TestSetupAndCleanup.RestoreReceiptDirectory();
        }

        [TestMethod]
        public void TestInitialState()
        {
            int productAmountSold = Convert.ToInt32(ReadData("SELECT amountSold FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(0, productAmountSold);
        }

        [TestMethod]
        public void TestAddOneProductSold()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));

            addProductButton.Click();
            checkoutButton.Click();
            Thread.Sleep(1000);

            int productAmountSold = Convert.ToInt32(ReadData("SELECT amountSold FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(1, productAmountSold);
        }

        [TestMethod]
        public void TestAddMultipleProductSold()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));

            for (int i = 0;i < 12; i++)
            {
                addProductButton.Click();
            }

            checkoutButton.Click();
            Thread.Sleep(1200);

            int productAmountSold = Convert.ToInt32(ReadData("SELECT amountSold FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(12, productAmountSold);
        }
    }
}
