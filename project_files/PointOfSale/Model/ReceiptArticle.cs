using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace PointOfSale.Model
{
    internal class ReceiptArticle
    {
        CultureInfo svSE = CultureInfo.CreateSpecificCulture("sv-SE");
        public Product Product
        { get; set; }
        public int Quantity
        { get; set; }

        public float Sum { get; set; }
        public string SumFormatted
        {
            get
            {
                return Sum.ToString("0.00", svSE);
            }
        }


        public ReceiptArticle(Article article)
        {
            Product = article.Product;
            Quantity = article.Quantity;
            Sum = article.Sum;
        }
    }
}
