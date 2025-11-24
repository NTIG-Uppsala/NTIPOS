using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    public class Article
    {
        public Product Product
        { get; set; }
        public int Quantity
        { get; set; }
        public int Sum
        { get; set; }

        public Article(Product product)
        {
            Product = product;
            Quantity = 1;
            Sum = Product.Price * Quantity;
        }
    }
}
