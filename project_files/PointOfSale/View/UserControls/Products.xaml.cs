using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static PointOfSale.MainWindow;

namespace PointOfSale.View.UserControls
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : UserControl
    {
        public Products()
        {
            /// Products List
            List<ProductCategory> categoryList =
            [
                new ProductCategory("Tobak", "tobakProducts", Brushes.SaddleBrown,      [new Product("Marlboro Red (20-pack)", "Tobak", 89),
                                                                                         new Product("Camel Blue (20-pack)", "Tobak", 85),
                                                                                         new Product("L&M Filter (20-pack)", "Tobak", 79),
                                                                                         new Product("Skruf Original Portion", "Tobak", 62),
                                                                                         new Product("Göteborgs Rapé White Portion", "Tobak", 67),
                ]),

                new ProductCategory("Godis", "godisProducts", Brushes.LightPink,        [new Product("Marabou Mjölkchoklad 100 g", "Godis", 25),
                                                                                         new Product("Daim dubbel", "Godis", 15),
                                                                                         new Product("Kexchoklad", "Godis", 12),
                                                                                         new Product("Malaco Gott & Blandat 160 g", "Godis", 28),
                ]),

                new ProductCategory("Enkel mat", "matProducts", Brushes.Orange,         [new Product("Korv med bröd", "Enkel mat", 89),
                                                                                         new Product("Varm toast (ost & skinka)", "Enkel mat", 30),
                                                                                         new Product("Pirog (köttfärs)", "Enkel mat", 22),
                                                                                         new Product("Färdig sallad (kyckling)", "Enkel mat", 49),
                                                                                         new Product("Panini (mozzarella & pesto)", "Enkel mat", 45),
                ]),

                new ProductCategory("Tidningar", "tidningProducts", Brushes.LightBlue,  [new Product("Aftonbladet (dagens)", "Tidningar", 28),
                                                                                         new Product("Expressen (dagens)", "Tidningar", 28),
                                                                                         new Product("Illustrerad Vetenskap", "Tidningar", 79),
                                                                                         new Product("Kalle Anka & Co", "Tidningar", 45),
                                                                                         new Product("Allt om Mat", "Tidningar", 69),
                ]),
            ];

            InitializeComponent();

            foreach (ProductCategory category in categoryList)
            {
                CategoryButton categoryButton = new()
                {
                    CategoryColor = category.Color,
                    CategoryName = category.Name
                };

                ListOfProducts listOfProducts = new() 
                { 
                    Name = category.Key
                };

                productCategories.Children.Add(categoryButton);
                productsGrid.Children.Add(listOfProducts);
            }
        }

        public class ProductCategory
        {
            public string Name
            { get; set; }

            public string Key
            { get; set; }

            public Brush Color
            { get; set; }

            public List<Product> ProductsList
            { get; set; }

            public ProductCategory(string categoryName, string categoryKey, Brush categoryColor, List<Product> categoryProductsList)
            {
                Name = categoryName;
                Key = categoryKey;
                Color = categoryColor;
                ProductsList = categoryProductsList;
            }
        }

        public class Product
        {
            public string Name
            { get; set; }
            public string Category
            { get; set; }
            public int Price
            { get; set; }

            public Product(string productName, string productCategory, int priceInt)
            {
                Name = productName;
                Category = productCategory;
                Price = priceInt;
            }
        }
    }
}
