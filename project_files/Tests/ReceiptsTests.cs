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
    public sealed class ReceiptsTests
    {
        private String applicationPath = Path.GetFullPath(@"..\..\..\..\PointOfSale\bin\Debug\net9.0-windows\PointOfSale.exe");
        private Application application;
        private Window mainWindow;
        private ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

        private string receiptsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/POS/receipts";

        [TestInitialize]
        public void Setup()
        {
            TestSetupAndCleanup.InitializeTestDatabase();
            TestSetupAndCleanup.ProtectUserReceipts();
            using (SQLiteConnection connection = new SQLiteConnection(TestSetupAndCleanup.connectionString))
            {
                connection.Open();

                using (var tx = connection.BeginTransaction())
                    using (var cmd = new SQLiteCommand(@"
                                INSERT INTO receipts(Id, Time)
                                VALUES (@id, @time)", connection, tx))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@id"));
                        cmd.Parameters.Add(new SQLiteParameter("@time"));

                        cmd.Parameters["@id"].Value = 0;
                        cmd.Parameters["@time"].Value = Convert.ToDateTime("2000-03-08");

                        cmd.ExecuteNonQuery();

                        tx.Commit();
                    }

                using (var tx = connection.BeginTransaction())
                    using (var cmd = new SQLiteCommand(@"
                                INSERT INTO receiptArticles(Name, Price, Quantity, ReceiptId)
                                VALUES (@name, @price, @quantity, @receiptId)", connection, tx))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@name"));
                        cmd.Parameters.Add(new SQLiteParameter("@price"));
                        cmd.Parameters.Add(new SQLiteParameter("@quantity"));
                        cmd.Parameters.Add(new SQLiteParameter("@receiptId"));

                        cmd.Parameters["@name"].Value = "Kebabpanini";
                        cmd.Parameters["@price"].Value = 73;
                        cmd.Parameters["@quantity"].Value = 4;
                        cmd.Parameters["@receiptId"].Value = 0;

                        cmd.ExecuteNonQuery();

                        tx.Commit();
                    }
            }

            // Start Application
            application = Application.Launch(applicationPath);
            mainWindow = application.GetMainWindow(new UIA3Automation());
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Close Application
            application.Close();

            TestSetupAndCleanup.RestoreReceiptDirectory();
            TestSetupAndCleanup.RemoveTestDatabase();
        }

        [TestMethod]
        public void TestInitialState()
        {
            var receiptsTab = mainWindow.FindFirstDescendant(cf.ByName("Kvitton"));

            receiptsTab.Click();
            var receiptList = mainWindow.FindFirstDescendant(cf.ByAutomationId("ReceiptsView"));
            var receipt = receiptList.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            Assert.IsTrue(receipt.FindFirstDescendant(cf.ByName("0")) is not null);
            Assert.IsTrue(receipt.FindFirstDescendant(cf.ByName("292,00")) is not null);
        }

        [TestMethod]
        public void AddReceipt()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));
            var receiptsTab = mainWindow.FindFirstDescendant(cf.ByName("Kvitton"));

            addProductButton.Click();

            checkoutButton.Click();

            receiptsTab.Click();
            var receiptList = mainWindow.FindFirstDescendant(cf.ByAutomationId("ReceiptsView"));
            var receipt = receiptList.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            Assert.IsTrue(receipt.FindFirstDescendant(cf.ByName("1")) is not null);
            Assert.IsTrue(receipt.FindFirstDescendant(cf.ByName("12,00")) is not null);

            Assert.IsTrue(Directory.Exists(receiptsDirectory));

            Assert.AreEqual(1, Directory.GetFiles(receiptsDirectory).Length);
        }

        [TestMethod]
        public void PrintOldReceipt()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kalle Anka & Co"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));
            var receiptsTab = mainWindow.FindFirstDescendant(cf.ByName("Kvitton"));

            addProductButton.Click();

            checkoutButton.Click();

            receiptsTab.Click();
            var receiptList = mainWindow.FindFirstDescendant(cf.ByAutomationId("ReceiptsView"));
            var receipt = receiptList.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            receipt.Click();

            var printButton = mainWindow.FindFirstDescendant(cf.ByName("Skriv ut"));
            printButton.Click();

            Thread.Sleep(1000);

            Assert.IsTrue(Directory.Exists(receiptsDirectory));

            Assert.AreEqual(2, Directory.GetFiles(receiptsDirectory).Length);
        }

        [TestMethod]
        public void SortReceipts()
        {
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kalle Anka & Co"));
            var checkoutButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("CheckoutButton"));
            var receiptsTab = mainWindow.FindFirstDescendant(cf.ByName("Kvitton"));

            // Add a receipt 
            addProductButton.Click();
            checkoutButton.Click();

            // Add a receipt with higer total
            addProductButton.Click();
            addProductButton.Click();
            addProductButton.Click();
            checkoutButton.Click();

            // Enter the Receipts view
            receiptsTab.Click();
            var receiptList = mainWindow.FindFirstDescendant(cf.ByAutomationId("ReceiptsView"));
            var firstReceipt = receiptList.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            Assert.IsTrue(firstReceipt.FindFirstDescendant(cf.ByName("2")) is not null);

            // Sort by ascending price
            var sortByTotalSumButton = receiptList.FindFirstDescendant(cf.ByName("Summa"));
            sortByTotalSumButton.Click();

            firstReceipt = receiptList.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            Assert.IsTrue(firstReceipt.FindFirstDescendant(cf.ByName("45,00")) is not null);

            // Sort by decending time
            var sortByTimeButton = receiptList.FindFirstDescendant(cf.ByName("Tid"));
            sortByTimeButton.Click();
            sortByTimeButton.Click();
            
            firstReceipt = receiptList.FindFirstDescendant(cf.ByClassName("DataGridRow"));

            Assert.IsTrue(firstReceipt.FindFirstDescendant(cf.ByName("2")) is not null);
        }
    }
}
