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
                string createProductsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS products(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            category TEXT NOT NULL,
                            price FLOAT NOT NULL,
                            amountSold INTEGER NOT NULL);";

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = createProductsTableQuery;
                    command.ExecuteNonQuery();
                }
            }

            var products = new[]
            {
                new { Name = "Marabou Mjölkchoklad 100 g", Category = "Godis", Price = 25.00, AmountSold = 0 },
                new { Name = "Daim dubbel", Category = "Godis", Price = 15.00, AmountSold = 0 },
                new { Name = "Kexchoklad", Category = "Godis", Price = 12.00, AmountSold = 0 },
                new { Name = "Malaco Gott & Blandat 160 g", Category = "Godis", Price = 28.00, AmountSold = 0 },

                new { Name = "Korv med bröd", Category = "Enkel mat", Price = 25.00, AmountSold = 0 },
                new { Name = "Varm toast (ost & skinka)", Category = "Enkel mat", Price = 30.00, AmountSold = 0 },
                new { Name = "Pirog (köttfärs)", Category = "Enkel mat", Price = 22.00, AmountSold = 0 },
                new { Name = "Färdig sallad (kyckling)", Category = "Enkel mat", Price = 49.00, AmountSold = 0 },
                new { Name = "Panini (mozzarella & pesto)", Category = "Enkel mat", Price = 45.00, AmountSold = 0 },

                new { Name = "Aftonbladet (dagens)", Category = "Tidningar", Price = 28.00, AmountSold = 0 },
                new { Name = "Expressen (dagens)", Category = "Tidningar", Price = 28.00, AmountSold = 0 },
                new { Name = "Illustrerad Vetenskap", Category = "Tidningar", Price = 79.00, AmountSold = 0 },
                new { Name = "Kalle Anka & Co", Category = "Tidningar", Price = 45.00, AmountSold = 0 },
                new { Name = "Allt om Mat", Category = "Tidningar", Price = 69.00, AmountSold = 0 },
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var tx = connection.BeginTransaction())
                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO products(Name, Category, Price, AmountSold)
                            VALUES (@name, @category, @price, @amountSold)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@name"));
                    cmd.Parameters.Add(new SQLiteParameter("@category"));
                    cmd.Parameters.Add(new SQLiteParameter("@price"));
                    cmd.Parameters.Add(new SQLiteParameter("@amountSold"));

                    foreach (var product in products)
                    {
                        cmd.Parameters["@name"].Value = product.Name;
                        cmd.Parameters["@category"].Value = product.Category;
                        cmd.Parameters["@price"].Value = product.Price;
                        cmd.Parameters["@amountSold"].Value = product.AmountSold;
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
    }
}
