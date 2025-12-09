using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    public class Article : ViewModelBase
    {
        CultureInfo svSE = CultureInfo.CreateSpecificCulture("sv-SE");
        public Product Product
        { get; set; }

        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { 
                quantity = value;
                NotifyPropertyChanged();
            }
        }

        private float sum;

        public float Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                NotifyPropertyChanged();
            }
        }

        public string SumFormatted
        {
            get
            {
                return Sum.ToString("0.00", svSE);
            }
        }

        public Article(Product product)
        {
            Product = product;
            Quantity = 1;
            Sum = Product.Price * Quantity;
        }

        public Article(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            Sum = Product.Price * Quantity;
        }
    }
}
