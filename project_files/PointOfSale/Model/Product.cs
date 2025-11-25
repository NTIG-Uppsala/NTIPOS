using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    public class Product
    {
        public string Name
        { get; set; }
        public string Category
        { get; set; }
        public int Price
        { get; set; }

        public Product(string productName, string categoryName, int priceInt)
        {
            Name = productName;
            Category = categoryName;
            Price = priceInt;
        }
    }
}
