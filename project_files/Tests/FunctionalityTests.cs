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
    public sealed class FunctionalityTests
    {
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
            var totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("TotalSum"));
            var articlesView = mainWindow.FindFirstDescendant(cf.ByAutomationId("ArticlesView"));

            Assert.AreEqual("0,00", totalSumBox.Name);
            Assert.AreEqual(null, articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem")));
        }

        [TestMethod]
        public void TestAddKexchoklad()
        {
            var totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("TotalSum"));
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var articlesView = mainWindow.FindFirstDescendant(cf.ByAutomationId("ArticlesView"));
            var firstArticle = articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem"));


            Assert.AreEqual("0,00", totalSumBox.Name);
            Assert.AreEqual(null, articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem")));

            addProductButton.Click();

            Assert.AreEqual("12,00", totalSumBox.Name);
        }

        [TestMethod]
        public void TestAddManyKexchoklad()
        {
            var totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("TotalSum"));
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var articlesView = mainWindow.FindFirstDescendant(cf.ByAutomationId("ArticlesView"));
            var firstArticle = articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem"));

            Assert.AreEqual("0,00", totalSumBox.Name);
            Assert.AreEqual(null, articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem")));

            for (int i = 0; i < 10; i++)
            {
                addProductButton.Click();
            }

            Assert.AreEqual("120,00", totalSumBox.Name);
        }

        [TestMethod]
        public void TestResetTotal()
        {
            var totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("TotalSum"));
            var addProductButton = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var articlesView = mainWindow.FindFirstDescendant(cf.ByAutomationId("ArticlesView"));
            var firstArticle = articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem"));
            var resetButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("AbortButton"));

            Assert.AreEqual("0,00", totalSumBox.Name);
            Assert.AreEqual(null, articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem")));

            for (int i = 0; i < 10; i++)
            {
                addProductButton.Click();
            }

            Assert.AreEqual("120,00", totalSumBox.Name);

            resetButton.Click();
            
            Assert.AreEqual("0,00", totalSumBox.Name);
        }

        [TestMethod]
        public void TestAddDifferentProducts()
        {
            var totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("TotalSum"));
            var addProductButton1 = mainWindow.FindFirstDescendant(cf.ByName("Kexchoklad"));
            var addProductButton2 = mainWindow.FindFirstDescendant(cf.ByName("Korv med bröd"));
            var addProductButton3 = mainWindow.FindFirstDescendant(cf.ByName("Kalle Anka & Co"));
            var articlesView = mainWindow.FindFirstDescendant(cf.ByAutomationId("ArticlesView"));
            var firstArticle = articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem"));
            var resetButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("AbortButton"));

            Assert.AreEqual("0,00", totalSumBox.Name);
            Assert.AreEqual(null, articlesView.FindFirstDescendant(cf.ByClassName("ListBoxItem")));

            addProductButton1.Click();
            Assert.AreEqual("12,00", totalSumBox.Name);

            addProductButton2.Click();
            Assert.AreEqual("37,00", totalSumBox.Name);

            addProductButton3.Click();
            Assert.AreEqual("82,00", totalSumBox.Name);

            resetButton.Click();
            Assert.AreEqual("0,00", totalSumBox.Name);
        }
    }
}
