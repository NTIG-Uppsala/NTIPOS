using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointOfSale.Model;
using PointOfSale.ViewModel;

namespace PointOfSale.MVVM
{
    class DatabaseHelper
    {
        private static readonly string fileLocation = @".\databases\POSDB.db";
        private static readonly string connectionString = @"Data Source=.\databases\POSDB.db;Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(fileLocation))
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
            }
        }

        public static void AddProducts()
        {
            var products = new[]
            {
                new { Name = "Marlboro Red (20-pack)", Category = "Tobak", Price = 89.00, Stock = 100 },
                new { Name = "Camel Blue (20-pack)", Category = "Tobak", Price = 85.00, Stock = 100 },
                new { Name = "L&M Filter (20-pack)", Category = "Tobak", Price = 79.00, Stock = 100 },
                new { Name = "Skruf Original Portion", Category = "Tobak", Price = 62.00, Stock = 100 },
                new { Name = "Göteborgs Rapé White Portion", Category = "Tobak", Price = 67.00, Stock = 100 },

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

        public static void RemoveStock(ObservableCollection<Article> articles)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                foreach (var article in articles)
                {
                    string query = $"UPDATE products SET stock = stock - '{article.Quantity}' WHERE name = '{article.Product.Name}'";
                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static bool CheckStock(Product product)
        {
            int increaseAmount = 1;
            int stockData = Convert.ToInt32(ReadData($"SELECT stock FROM products WHERE name = '{product.Name}'"));
            Article article = ArticlesViewModel.ArticlesVM.Articles.SingleOrDefault(article => article.Product.Name == product.Name);

            if (article != null) 
            {
                increaseAmount += article.Quantity;
            }

            if (stockData - increaseAmount >= 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
