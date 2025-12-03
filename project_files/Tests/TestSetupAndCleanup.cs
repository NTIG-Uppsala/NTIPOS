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

        private static readonly string fileLocation = @".\databases\POSDB.db";
        public static readonly string connectionString = @"Data Source=.\databases\POSDB.db;Version=3;";

        public static void InitializeTestDatabase()
        {
            FileInfo file = new FileInfo(fileLocation);
            file.Directory.Create();
            SQLiteConnection.CreateFile(fileLocation);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createProductsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS products(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            category TEXT NOT NULL,
                            price FLOAT NOT NULL,
                            stock INTEGER NOT NULL);";

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = createProductsTableQuery;
                    command.ExecuteNonQuery();
                }
            }

            var products = new[]
            {
                new { Name = "Marabou Mjölkchoklad 100 g", Category = "Godis", Price = 25.00, Stock = 100 },
                new { Name = "Daim dubbel", Category = "Godis", Price = 15.00, Stock = 100 },
                new { Name = "Kexchoklad", Category = "Godis", Price = 12.00, Stock = 100 },
                new { Name = "Malaco Gott & Blandat 160 g", Category = "Godis", Price = 28.00, Stock = 100 },

                new { Name = "Korv med bröd", Category = "Enkel mat", Price = 25.00, Stock = 100 },
                new { Name = "Varm toast (ost & skinka)", Category = "Enkel mat", Price = 30.00, Stock = 100 },
                new { Name = "Pirog (köttfärs)", Category = "Enkel mat", Price = 22.00, Stock = 100 },
                new { Name = "Färdig sallad (kyckling)", Category = "Enkel mat", Price = 49.00, Stock = 100 },
                new { Name = "Panini (mozzarella & pesto)", Category = "Enkel mat", Price = 45.00, Stock = 100 },

                new { Name = "Aftonbladet (dagens)", Category = "Tidningar", Price = 28.00, Stock = 100 },
                new { Name = "Expressen (dagens)", Category = "Tidningar", Price = 28.00, Stock = 100 },
                new { Name = "Illustrerad Vetenskap", Category = "Tidningar", Price = 79.00, Stock = 100 },
                new { Name = "Kalle Anka & Co", Category = "Tidningar", Price = 45.00, Stock = 100 },
                new { Name = "Allt om Mat", Category = "Tidningar", Price = 69.00, Stock = 100 },
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var tx = connection.BeginTransaction())
                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO products(Name, Category, Price, Stock)
                            VALUES (@name, @category, @price, @stock)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@name"));
                    cmd.Parameters.Add(new SQLiteParameter("@category"));
                    cmd.Parameters.Add(new SQLiteParameter("@price"));
                    cmd.Parameters.Add(new SQLiteParameter("@stock"));

                    foreach (var product in products)
                    {
                        cmd.Parameters["@name"].Value = product.Name;
                        cmd.Parameters["@category"].Value = product.Category;
                        cmd.Parameters["@price"].Value = product.Price;
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
                Directory.Delete(@".\databases", true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured " + e.Message);
            }
        }
        
        public static void RemoveTestReceiptDirectory()
        {
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
    }
}
