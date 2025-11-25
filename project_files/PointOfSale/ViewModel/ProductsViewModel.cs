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
                new ("Marlboro Red (20-pack)", "Tobak", 89),
                new ("Camel Blue (20-pack)", "Tobak", 85),
                new ("L&M Filter (20-pack)", "Tobak", 79),
                new ("Skruf Original Portion", "Tobak", 62),
                new ("Göteborgs Rapé White Portion", "Tobak", 67),

                new ("Marabou Mjölkchoklad 100 g", "Godis", 25),
                new ("Daim dubbel", "Godis", 15),
                new ("Kexchoklad", "Godis", 12),
                new ("Malaco Gott & Blandat 160 g", "Godis", 28),

                new ("Korv med bröd", "Enkel mat", 25),
                new ("Varm toast (ost & skinka)", "Enkel mat", 30),
                new ("Pirog (köttfärs)", "Enkel mat", 22),
                new ("Färdig sallad (kyckling)", "Enkel mat", 49),
                new ("Panini (mozzarella & pesto)", "Enkel mat", 45),

                new ("Aftonbladet (dagens)", "Tidningar", 28),
                new ("Expressen (dagens)", "Tidningar", 28),
                new ("Illustrerad Vetenskap", "Tidningar", 79),
                new ("Kalle Anka & Co", "Tidningar", 45),
                new ("Allt om Mat", "Tidningar", 69),
            };
        }
    }
}
