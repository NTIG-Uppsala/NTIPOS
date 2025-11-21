using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PointOfSale.ViewModel
{
    public class ProductsViewModel
    {
        public ObservableCollection<Product> Products { get; set; }

        public ProductsViewModel()
        {
            Products = new ObservableCollection<Product>
            {
                new Product("Marlboro Red (20-pack)", "Tobak", 89),
                new Product("Camel Blue (20-pack)", "Tobak", 85),
                new Product("L&M Filter (20-pack)", "Tobak", 79),
                new Product("Skruf Original Portion", "Tobak", 62),
                new Product("Göteborgs Rapé White Portion", "Tobak", 67),

                new Product("Marabou Mjölkchoklad 100 g", "Godis", 25),
                new Product("Daim dubbel", "Godis", 15),
                new Product("Kexchoklad", "Godis", 12),
                new Product("Malaco Gott & Blandat 160 g", "Godis", 28),

                new Product("Korv med bröd", "Enkel mat", 89),
                new Product("Varm toast (ost & skinka)", "Enkel mat", 30),
                new Product("Pirog (köttfärs)", "Enkel mat", 22),
                new Product("Färdig sallad (kyckling)", "Enkel mat", 49),
                new Product("Panini (mozzarella & pesto)", "Enkel mat", 45),

                new Product("Aftonbladet (dagens)", "Tidningar", 28),
                new Product("Expressen (dagens)", "Tidningar", 28),
                new Product("Illustrerad Vetenskap", "Tidningar", 79),
                new Product("Kalle Anka & Co", "Tidningar", 45),
                new Product("Allt om Mat", "Tidningar", 69),
            };
        }
    }
}
