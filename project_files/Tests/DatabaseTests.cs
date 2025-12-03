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

            application = Application.Launch(applicationPath);
            mainWindow = application.GetMainWindow(new UIA3Automation());
        }

        [TestCleanup]
        public void Cleanup()
        {
            application.Close();
            TestSetupAndCleanup.RemoveTestDatabase();
            TestSetupAndCleanup.RemoveTestReceiptDirectory();
        }

        [TestMethod]
        public void TestInitialState()
        {
            int productStock = Convert.ToInt32(ReadData("SELECT stock FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(100, productStock);
        }

        [TestMethod]
        public void TestRemoveOneStock()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));

            addProductButton.Click();
            checkoutButton.Click();
            Thread.Sleep(1000);

            int productStock = Convert.ToInt32(ReadData("SELECT stock FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(99, productStock);
        }

        [TestMethod]
        public void TestRemoveMultipleStock()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));

            for (int i = 0;i < 12; i++)
            {
                addProductButton.Click();
            }

            checkoutButton.Click();
            Thread.Sleep(1000);

            int productStock = Convert.ToInt32(ReadData("SELECT stock FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(88, productStock);
        }

        [TestMethod]
        public void TestOutOfStock()
        {
            using (SQLiteConnection connection = new SQLiteConnection(TestSetupAndCleanup.connectionString))
            {
                connection.Open();
                string query = $"UPDATE products SET stock = '10' WHERE name = 'Kexchoklad'";
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));

            for (int i = 0; i < 11; i++)
            {
                addProductButton.Click();
            }

            var popUpWindow = mainWindow.FindFirstDescendant(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            var yesButton = popUpWindow.FindFirstDescendant(cf.ByName("Ja"));

            yesButton.Click();

            checkoutButton.Click();
            Thread.Sleep(1000);

            int productStock = Convert.ToInt32(ReadData("SELECT stock FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(-1, productStock);

            addProductButton.Click();

            popUpWindow = mainWindow.FindFirstDescendant(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
            var noButton = popUpWindow.FindFirstDescendant(cf.ByName("Nej"));
            
            noButton.Click();

            checkoutButton.Click();
            Thread.Sleep(1000);

            productStock = Convert.ToInt32(ReadData("SELECT stock FROM products WHERE name = 'Kexchoklad'"));
            Assert.AreEqual(-1, productStock);
        }
    }
}
