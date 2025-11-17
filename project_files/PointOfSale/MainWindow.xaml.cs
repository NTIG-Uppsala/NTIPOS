using System.Diagnostics;
using System.Text;
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

namespace PointOfSale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /// Products List
            List<Product> productsList = new List<Product>();

            productsList.Add(new Product("Marlboro Red (20-pack)", "Tobak", 89));
            productsList.Add(new Product("Camel Blue (20-pack)", "Tobak", 85));
            productsList.Add(new Product("L&M Filter (20-pack)", "Tobak", 79));
            productsList.Add(new Product("Skruf Original Portion", "Tobak", 62));
            productsList.Add(new Product("Göteborgs Rapé White Portion", "Tobak", 67));

            productsList.Add(new Product("Marabou Mjölkchoklad 100 g", "Godis", 25));
            productsList.Add(new Product("Daim dubbel", "Godis", 15));
            productsList.Add(new Product("Kexchoklad", "Godis", 12));
            productsList.Add(new Product("Malaco Gott & Blandat 160 g", "Godis", 28));

            productsList.Add(new Product("Korv med bröd", "Enkel mat", 89));
            productsList.Add(new Product("Varm toast (ost & skinka)", "Enkel mat", 30));
            productsList.Add(new Product("Pirog (köttfärs)", "Enkel mat", 22));
            productsList.Add(new Product("Färdig sallad (kyckling)", "Enkel mat", 49));
            productsList.Add(new Product("Panini (mozzarella & pesto)", "Enkel mat", 45));

            productsList.Add(new Product("Aftonbladet (dagens)", "Tidningar", 28));
            productsList.Add(new Product("Expressen (dagens)", "Tidningar", 28));
            productsList.Add(new Product("Illustrerad Vetenskap", "Tidningar", 79));
            productsList.Add(new Product("Kalle Anka & Co", "Tidningar", 45));
            productsList.Add(new Product("Allt om Mat", "Tidningar", 69));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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