using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal static class TestSetupAndCleanup
    {

        private static readonly string databaseLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/POS/databases/POSDB.db";
        private static readonly string tempDatabaseLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/POS/databases/tempPOSDB.db";
        public static readonly string connectionString = "Data Source=" + databaseLocation + ";Version=3;";

        public static void Login(Window mainWindow, ConditionFactory cf)
        {
            var userIDInput = mainWindow.FindFirstDescendant(cf.ByAutomationId("UserIDInput")).AsTextBox();
            var loginButton = mainWindow.FindFirstDescendant(cf.ByAutomationId("LoginButton"));
            var saleTab = mainWindow.FindFirstDescendant(cf.ByName("Försäljning"));

            userIDInput.Click();
            userIDInput.Enter("testID");
            loginButton.Click();

            saleTab.Click();
        }

        public static void InitializeTestDatabase()
        {
            try
            {
                if (File.Exists(databaseLocation))
                    File.Move(databaseLocation, tempDatabaseLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }

            FileInfo file = new FileInfo(databaseLocation);
            file.Directory.Create();
            SQLiteConnection.CreateFile(databaseLocation);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createCategoriesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS categories(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            categoryName TEXT NOT NULL,
                            categoryColor TEXT NOT NULL
                    );";

                string createProductsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS products(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            categoryId INTEGER NOT NULL,
                            price FLOAT NOT NULL,
                            amountSold INTEGER NOT NULL,
                            stock INTEGER NOT NULL,
                            FOREIGN KEY (categoryId) REFERENCES categories(id)
                    );";

                string createReceiptsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS receipts(
                            id INTEGER PRIMARY KEY,
                            time TEXT NOT NULL
                    );";

                string createReceiptArticlesTebleQuery = @"
                    CREATE TABLE IF NOT EXISTS receiptArticles(
                            id INTEGER PRIMARY KEY AUTOINCREMENT, 
                            name TEXT NOT NULL, 
                            price FLOAT NOT NULL, 
                            quantity INTEGER NOT NULL, 
                            receiptId INTEGER NOT NULL,
                            FOREIGN KEY (receiptId) REFERENCES receipts(id)
                    );";

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = createProductsTableQuery;
                    command.ExecuteNonQuery();

                    command.CommandText = createCategoriesTableQuery;
                    command.ExecuteNonQuery();
                    
                    command.CommandText = createReceiptsTableQuery;
                    command.ExecuteNonQuery();
                    
                    command.CommandText = createReceiptArticlesTebleQuery;
                    command.ExecuteNonQuery();
                }
            }

            var categories = new[]
            {
                new { Name = "Godis", Color = "LightPink"},
                new { Name = "Enkel mat", Color = "Orange"},
                new { Name = "Tidningar", Color = "LightBlue"},
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var tx = connection.BeginTransaction())
                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO categories(CategoryName, CategoryColor)
                            VALUES (@categoryName, @categoryColor)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@categoryName"));
                    cmd.Parameters.Add(new SQLiteParameter("@categoryColor"));

                    foreach (var category in categories)
                    {
                        cmd.Parameters["@categoryName"].Value = category.Name;
                        cmd.Parameters["@categoryColor"].Value = category.Color;
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }

            var products = new[]
            {
                new { Name = "Marabou Mjölkchoklad 100 g", CategoryID = 2, Price = 25.00, Stock = 100 },
                new { Name = "Daim dubbel", CategoryID = 2, Price = 15.00, Stock = 100 },
                new { Name = "Kexchoklad", CategoryID = 2, Price = 12.00, Stock = 100 },
                new { Name = "Malaco Gott & Blandat 160 g", CategoryID = 2, Price = 28.00, Stock = 100 },

                new { Name = "Korv med bröd", CategoryID = 3, Price = 25.00, Stock = 100 },
                new { Name = "Varm toast (ost & skinka)", CategoryID = 3, Price = 30.00, Stock = 100 },
                new { Name = "Pirog (köttfärs)", CategoryID = 3, Price = 22.00, Stock = 100 },
                new { Name = "Färdig sallad (kyckling)", CategoryID = 3, Price = 49.00, Stock = 100 },
                new { Name = "Panini (mozzarella & pesto)", CategoryID = 3, Price = 45.00, Stock = 100 },

                new { Name = "Aftonbladet (dagens)", CategoryID = 4, Price = 28.00, Stock = 100 },
                new { Name = "Expressen (dagens)", CategoryID = 4, Price = 28.00, Stock = 100 },
                new { Name = "Illustrerad Vetenskap", CategoryID = 4, Price = 79.00, Stock = 100 },
                new { Name = "Kalle Anka & Co", CategoryID = 4, Price = 45.00, Stock = 100 },
                new { Name = "Allt om Mat", CategoryID = 4, Price = 69.00, Stock = 100 },
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                //Adds each and every product from the products array to the database with their required fields
                using (var tx = connection.BeginTransaction())
                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO products(Name, CategoryID, Price, AmountSold, Stock)
                            VALUES (@name, @categoryId, @price, @amountSold, @stock)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@name"));
                    cmd.Parameters.Add(new SQLiteParameter("@categoryId"));
                    cmd.Parameters.Add(new SQLiteParameter("@price"));
                    cmd.Parameters.Add(new SQLiteParameter("@amountSold"));
                    cmd.Parameters.Add(new SQLiteParameter("@stock"));

                    foreach (var product in products)
                    {
                        cmd.Parameters["@name"].Value = product.Name;
                        cmd.Parameters["@categoryId"].Value = product.CategoryID;
                        cmd.Parameters["@price"].Value = product.Price;
                        cmd.Parameters["@amountSold"].Value = 0;
                        cmd.Parameters["@stock"].Value = product.Stock;

                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
        }

        public static void RemoveTestDatabase()
        {
            try
            {
                if (File.Exists(databaseLocation))
                    File.Delete(databaseLocation);

                if (File.Exists(tempDatabaseLocation))
                    File.Move(tempDatabaseLocation, databaseLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }
        }
        
        private static readonly string receiptsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/POS/receipts";
        private static readonly string tempReceiptsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/POS/treceipts";

        public static void ProtectUserReceipts()
        {
            try
            {
                if (Directory.Exists(receiptsDirectory))
                    Directory.Move(receiptsDirectory, tempReceiptsDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }
        }
        public static void RestoreReceiptDirectory()
        {
            try
            {
                if (Directory.Exists(receiptsDirectory))
                    Directory.Delete(receiptsDirectory, true);

                if (Directory.Exists(tempReceiptsDirectory))
                    Directory.Move(tempReceiptsDirectory, receiptsDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }
        }

        private static readonly string configuration = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/POS/configuration.txt";
        private static readonly string tempConfiguration = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/POS/tconfiguration.txt";

        public static void ProtectUserConfiguration()
        {
            try
            {
                if (File.Exists(configuration))
                    File.Move(configuration, tempConfiguration);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }

            FileInfo file = new FileInfo(configuration);
            file.Directory.Create();
        }
        public static void RestoreUserConfiguration()
        {
            try
            {
                if (File.Exists(configuration))
                    File.Delete(configuration);

                if (File.Exists(tempConfiguration))
                    File.Move(tempConfiguration, configuration);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }
        }
        public static string ReadData(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    return result == null ? "" : result.ToString();
                }
            }
        }
    }
}
