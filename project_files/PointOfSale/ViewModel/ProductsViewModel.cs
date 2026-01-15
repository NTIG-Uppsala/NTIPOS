using PointOfSale.Model;
using PointOfSale.MVVM;
using PointOfSale.View.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PointOfSale.ViewModel
{
    public class ProductsViewModel : ViewModelBase
    {
        private static ProductsViewModel productsVm = new ProductsViewModel();
        public static ProductsViewModel ProductsVM { get { return productsVm; } }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> DataGridProducts { get; set; }

        private bool isLoggedIn;

        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set
            {
                isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }

        public ProductsViewModel()
        {
            Products = new ObservableCollection<Product>();
            DataGridProducts = new ObservableCollection<Product>();

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
            DataGridProducts.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT *, categoryName, categoryColor FROM products INNER JOIN categories ON products.categoryId=categories.id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("CategoryName")),
                                reader.GetFloat(reader.GetOrdinal("Price")),
                                reader.GetString(reader.GetOrdinal("CategoryColor")),
                                reader.GetInt32(reader.GetOrdinal("AmountSold"))
                                );

                        Products.Add(product);
                        DataGridProducts.Add(product);
                    }
                }
            }
        }
        
        public void ProductButtonMethod(Product product)
        {
            ArticlesViewModel.ArticlesVM.AddProduct(product);
        }

        public void UpdateStock()
        {
            foreach (Product product in ProductsViewModel.ProductsVM.Products)
            {
                product.Stock -= product.AmountSold;
                DatabaseHelper.ResetAmountSold(product);
            }
        }
    }
}
