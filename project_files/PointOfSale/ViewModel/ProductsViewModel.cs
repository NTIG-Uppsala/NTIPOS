using PointOfSale.Model;
using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PointOfSale.ViewModel
{
    public class ProductsViewModel
    {
        private static ProductsViewModel productsVm = new ProductsViewModel();
        public static ProductsViewModel ProductsVM { get { return productsVm; } }
        public ObservableCollection<Product> Products { get; set; }

        public ProductsViewModel()
        {
            Products = new ObservableCollection<Product>();
            if (!File.Exists(DatabaseHelper.fileLocation))
            {
                DatabaseHelper.InitializeDatabase();
                DatabaseHelper.AddCategories();
                DatabaseHelper.AddProducts();
            }
            getAllProducts(DatabaseHelper.connectionString);
        }

        public void getAllProducts(string connectionString)
        {
            Products.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM products";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Products.Add(new Product(
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Category")),
                                    reader.GetFloat(reader.GetOrdinal("Price")),
                                    reader.GetString(reader.GetOrdinal("Color"))
                                    ));
                    }
                }
            }
        }
    }
}
