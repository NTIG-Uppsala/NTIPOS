using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        public int Stock
        { get; set; }

        public Product(int id, string productName, string categoryName, float price, int stock)
        {
            Id = id;
            Name = productName;
            Category = categoryName;
            Price = price;
            Stock = stock;
        }
    }
}
