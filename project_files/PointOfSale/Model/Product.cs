using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PointOfSale.Model
{
    public class Product
    {
        CultureInfo svSE = CultureInfo.CreateSpecificCulture("sv-SE");
        public int Id
        { get; set; }

        public string Name
        { get; set; }

        public string Category
        { get; set; }

        public float Price { get; set; }
        public string PriceFormatted
        {
            get
            {
                return Price.ToString("0.00", svSE);
            }
        }

        public Brush Color
        { get; set; }

        public Product(int id, string productName, string categoryName, float price, string colorString)
        {
            Id = id;
            Name = productName;
            Category = categoryName;
            Price = price;
            Color = (Brush) new BrushConverter().ConvertFromString(colorString);
        }
    }
}
