using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    public class Article : ViewModelBase
    {
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

        private int sum;

        public int Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                NotifyPropertyChanged();
            }
        }

        public Article(Product product)
        {
            Product = product;
            Quantity = 1;
            Sum = Product.Price * Quantity;
        }
    }
}
