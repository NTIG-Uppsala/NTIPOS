using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core;
using System.Drawing.Text;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;

namespace Tests
{
    [TestClass]
    public sealed class ReceiptsTests
    {
        private String applicationPath = Path.GetFullPath(@"..\..\..\..\PointOfSale\bin\Debug\net9.0-windows\PointOfSale.exe");
        private Application application;
        private Window mainWindow;
        private ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

        [TestInitialize]
        public void Setup()
        {
            // Start Application
            application = Application.Launch(applicationPath);
            mainWindow = application.GetMainWindow(new UIA3Automation());
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Close Application
            application.Close();

            // Remove test receipts directory
            try
            {
                if (Directory.Exists(@".\receipts"))
                    Directory.Delete(@".\receipts", true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }
        }

        [TestMethod]
        public void TestInitialState()
        {
            var receiptsTab = mainWindow.FindFirstDescendant(cf.ByName("Kvitton"));

            receiptsTab.Click();
            var receiptList = mainWindow.FindFirstDescendant(cf.ByAutomationId("ReceiptsView"));

            Assert.AreEqual(null, receiptList.FindFirstDescendant(cf.ByClassName("ListBoxItem")));
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
            var receipt = receiptList.FindFirstDescendant(cf.ByClassName("ListBoxItem"));

            Assert.AreEqual("0", receipt.FindFirstDescendant(cf.ByAutomationId("ReceiptID")).Name);
            Assert.AreEqual("12,00", receipt.FindFirstDescendant(cf.ByAutomationId("ReceiptSum")).Name);

            Assert.IsTrue(Directory.Exists(@".\receipts"));

            Assert.AreEqual(1, Directory.GetFiles(@".\receipts").Length);
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
            var receipt = receiptList.FindFirstDescendant(cf.ByClassName("ListBoxItem"));

            receipt.Click();

            var printButton = mainWindow.FindFirstDescendant(cf.ByName("Skriv ut"));
            printButton.Click();

            Assert.IsTrue(Directory.Exists(@".\receipts"));

            Assert.AreEqual(2, Directory.GetFiles(@".\receipts").Length);
        }
    }
}
