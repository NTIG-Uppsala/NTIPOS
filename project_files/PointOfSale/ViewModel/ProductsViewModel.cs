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
        public List<Category> Categories = new List<Category>();

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
            getAllCategories(DatabaseHelper.connectionString);
            getAllProducts(DatabaseHelper.connectionString);
            APIHelper.FetchData();
        }

        public void getAllProducts(string connectionString)
        {
            Products.Clear();
            DataGridProducts.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM products";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetFloat(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("AmountSold")),
                                reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                reader.GetInt32(reader.GetOrdinal("Stock"))
                                );

                        Category productCategory = Categories.Find(x => x.Id == product.CategoryId);
                        productCategory = productCategory != null ? productCategory : Categories[Categories.Count-1];

                        product.Category = productCategory.Name;
                        product.Color = productCategory.Color;

                        Products.Add(product);
                        DataGridProducts.Add(product);
                    }
                }
            }
        }
        
        public void getAllCategories(string connectionString)
        {
            Categories.Clear();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM categories";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category category = new Category(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("CategoryName")),
                                reader.GetString(reader.GetOrdinal("CategoryColor"))
                                );

                        Categories.Add(category);
                    }
                }
            }
            Category defaultCategory = new Category(
                    0,
                    "Övrigt",
                    "White"
                    );

            Categories.Add(defaultCategory);
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
