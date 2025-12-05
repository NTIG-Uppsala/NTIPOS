using PointOfSale.Model;
using PointOfSale.View.UserControls;
using PointOfSale.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.MVVM
{
    class DatabaseHelper
    {
        public static readonly string fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/POS/databases/POSDB.db";
        public static readonly string connectionString = "Data Source=" + fileLocation + ";Version=3;";

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
                            category TEXT,
                            color TEXT,
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
            }
        }

        public static void AddCategories()
        {
            var categories = new[]
            {
                new { Name = "Tobak", Color = "SaddleBrown"},
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
        }

        public static void AddProducts()
        {
            var products = new[]
            {
                new { Name = "Marlboro Red (20-pack)", CategoryID = 1, Price = 89.00 },
                new { Name = "Camel Blue (20-pack)", CategoryID = 1, Price = 85.00 },
                new { Name = "L&M Filter (20-pack)", CategoryID = 1, Price = 79.00 },
                new { Name = "Skruf Original Portion", CategoryID = 1, Price = 62.00 },
                new { Name = "Göteborgs Rapé White Portion", CategoryID = 1, Price = 67.00 },

                new { Name = "Marabou Mjölkchoklad 100 g", CategoryID = 2, Price = 25.00 },
                new { Name = "Daim dubbel", CategoryID = 2, Price = 15.00 },
                new { Name = "Kexchoklad", CategoryID = 2, Price = 12.00 },
                new { Name = "Malaco Gott & Blandat 160 g", CategoryID = 2, Price = 28.00 },

                new { Name = "Korv med bröd", CategoryID = 3, Price = 25.00 },
                new { Name = "Varm toast (ost & skinka)", CategoryID = 3, Price = 30.00 },
                new { Name = "Pirog (köttfärs)", CategoryID = 3, Price = 22.00 },
                new { Name = "Färdig sallad (kyckling)", CategoryID = 3, Price = 49.00 },
                new { Name = "Panini (mozzarella & pesto)", CategoryID = 3, Price = 45.00 },

                new { Name = "Aftonbladet (dagens)", CategoryID = 4, Price = 28.00 },
                new { Name = "Expressen (dagens)", CategoryID = 4, Price = 28.00 },
                new { Name = "Illustrerad Vetenskap", CategoryID = 4, Price = 79.00 },
                new { Name = "Kalle Anka & Co", CategoryID = 4, Price = 45.00 },
                new { Name = "Allt om Mat", CategoryID = 4, Price = 69.00 },
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

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

                foreach (var product in products)
                {
                    string category = ReadData($"SELECT categoryName FROM products INNER JOIN categories ON products.categoryId=categories.id WHERE name = '{product.Name}'");

                    string colorString = ReadData($"SELECT categoryColor FROM products INNER JOIN categories ON products.categoryId=categories.id WHERE name = '{product.Name}'");

                    using (var cmd = new SQLiteCommand($"UPDATE products SET category = '{category}' WHERE name = '{product.Name}'", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new SQLiteCommand($"UPDATE products SET color = '{colorString}' WHERE name = '{product.Name}'", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
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

        public static void AddAmountSold(ObservableCollection<Article> articles)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                foreach (var article in articles)
                {
                    string query = $"UPDATE products SET amountSold = amountSold + '{article.Quantity}' WHERE name = '{article.Product.Name}'";
                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
