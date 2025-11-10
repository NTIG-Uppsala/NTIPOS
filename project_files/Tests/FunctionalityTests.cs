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
    public sealed class FunctionalityTests
    {
        private String applicationPath = Path.GetFullPath(@"..\..\..\..\PointOfSale\bin\Debug\net9.0-windows\PointOfSale.exe");
        private Application application;
        private Window mainWindow;
        private ConditionFactory cf;

        [TestInitialize]
        public void Setup()
        {
            application = Application.Launch(applicationPath);
            mainWindow = application.GetMainWindow(new UIA3Automation());
            cf = new ConditionFactory(new UIA3PropertyLibrary());
        }

        [TestCleanup]
        public void Cleanup()
        {
            application.Close();
        }
        [TestMethod]
        public void TestInitialState()
        {
            TextBox coffeeAmountBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("amount")).AsTextBox();
            TextBox totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("totalSum")).AsTextBox();

            Assert.AreEqual("0", coffeeAmountBox.Name);
            Assert.AreEqual("0", totalSumBox.Name);
        }

        [TestMethod]
        public void TestAddCoffee()
        {
            Button addCoffeeButton = mainWindow.FindFirstDescendant(cf.ByName("+")).AsButton();
            TextBox coffeeAmountBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("amount")).AsTextBox();
            TextBox totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("totalSum")).AsTextBox();
            
            Assert.AreEqual("0", coffeeAmountBox.Name);
            Assert.AreEqual("0", totalSumBox.Name);

            addCoffeeButton.Click();

            Assert.AreEqual("1", coffeeAmountBox.Name);
            Assert.AreEqual("49", totalSumBox.Name);
        }

        [TestMethod]
        public void TestRemoveCoffee()
        {
            Button addCoffeeButton = mainWindow.FindFirstDescendant(cf.ByName("+")).AsButton();
            Button removeCoffeeButton = mainWindow.FindFirstDescendant(cf.ByName("-")).AsButton();
            TextBox coffeeAmountBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("amount")).AsTextBox();
            TextBox totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("totalSum")).AsTextBox();

            addCoffeeButton.Click();

            Assert.AreEqual("1", coffeeAmountBox.Name);
            Assert.AreEqual("49", totalSumBox.Name);

            removeCoffeeButton.Click();

            Assert.AreEqual("0", coffeeAmountBox.Name);
            Assert.AreEqual("0", totalSumBox.Name);

            removeCoffeeButton.Click();

            Assert.AreEqual("0", coffeeAmountBox.Name);
            Assert.AreEqual("0", totalSumBox.Name);
        }

        [TestMethod]
        public void TestAddManyCoffee()
        {
            Button addCoffeeButton = mainWindow.FindFirstDescendant(cf.ByName("+")).AsButton();
            TextBox coffeeAmountBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("amount")).AsTextBox();
            TextBox totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("totalSum")).AsTextBox();
            
            Assert.AreEqual("0", coffeeAmountBox.Name);
            Assert.AreEqual("0", totalSumBox.Name);

            addCoffeeButton.Click();
            addCoffeeButton.Click();
            addCoffeeButton.Click();
            addCoffeeButton.Click();
            addCoffeeButton.Click();

            Assert.AreEqual("5", coffeeAmountBox.Name);
            Assert.AreEqual("245", totalSumBox.Name);
        }

        [TestMethod]
        public void TestResetTotal()
        {
            Button addCoffeeButton = mainWindow.FindFirstDescendant(cf.ByName("+")).AsButton();
            Button resetButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("resetButton")).AsButton();
            TextBox coffeeAmountBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("amount")).AsTextBox();
            TextBox totalSumBox = mainWindow.FindFirstDescendant(cf.ByAutomationId("totalSum")).AsTextBox();
            
            addCoffeeButton.Click();

            Assert.AreEqual("1", coffeeAmountBox.Name);
            Assert.AreEqual("49", totalSumBox.Name);

            resetButton.Click();
            
            Assert.AreEqual("0", coffeeAmountBox.Name);
            Assert.AreEqual("0", totalSumBox.Name);
        }
    }
}
