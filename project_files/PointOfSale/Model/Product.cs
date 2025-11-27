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

        public float Price { get; set; }

        public Product(string productName, string categoryName, float priceInt)
        {
            Name = productName;
            Category = categoryName;
            Price = priceInt;
        }
    }
}
