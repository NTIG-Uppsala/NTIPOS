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
                            FOREIGN KEY (categoryId) REFERENCES categories(id)
                    );";

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = createProductsTableQuery;
                    command.ExecuteNonQuery();

                    command.CommandText = createCategoriesTableQuery;
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
                new { Name = "Marabou Mjölkchoklad 100 g", CategoryID = 1, Price = 25.00 },
                new { Name = "Daim dubbel", CategoryID = 1, Price = 15.00 },
                new { Name = "Kexchoklad", CategoryID = 1, Price = 12.00 },
                new { Name = "Malaco Gott & Blandat 160 g", CategoryID = 1, Price = 28.00 },

                new { Name = "Korv med bröd", CategoryID = 2, Price = 25.00 },
                new { Name = "Varm toast (ost & skinka)", CategoryID = 2, Price = 30.00 },
                new { Name = "Pirog (köttfärs)", CategoryID = 2, Price = 22.00 },
                new { Name = "Färdig sallad (kyckling)", CategoryID = 2, Price = 49.00 },
                new { Name = "Panini (mozzarella & pesto)", CategoryID = 2, Price = 45.00 },

                new { Name = "Aftonbladet (dagens)", CategoryID = 3, Price = 28.00 },
                new { Name = "Expressen (dagens)", CategoryID = 3, Price = 28.00 },
                new { Name = "Illustrerad Vetenskap", CategoryID = 3, Price = 79.00 },
                new { Name = "Kalle Anka & Co", CategoryID = 3, Price = 45.00 },
                new { Name = "Allt om Mat", CategoryID = 3, Price = 69.00 },
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                //Adds each and every product from the products array to the database with their required fields
                using (var tx = connection.BeginTransaction())
                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO products(Name, CategoryID, Price, AmountSold)
                            VALUES (@name, @categoryId, @price, @amountSold)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@name"));
                    cmd.Parameters.Add(new SQLiteParameter("@categoryId"));
                    cmd.Parameters.Add(new SQLiteParameter("@price"));
                    cmd.Parameters.Add(new SQLiteParameter("@amountSold"));

                    foreach (var product in products)
                    {
                        cmd.Parameters["@name"].Value = product.Name;
                        cmd.Parameters["@categoryId"].Value = product.CategoryID;
                        cmd.Parameters["@price"].Value = product.Price;
                        cmd.Parameters["@amountSold"].Value = 0;

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
