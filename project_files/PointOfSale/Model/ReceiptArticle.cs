using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    internal class ReceiptArticle
    {
        public Product Product
        { get; set; }

        public int Quantity
        { get; set; }

        public int Sum
        { get; set; }

        public ReceiptArticle(Article article)
        {
            Product = article.Product;
            Quantity = article.Quantity;
            Sum = article.Sum;
        }
    }
}
