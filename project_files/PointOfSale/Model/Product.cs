using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PointOfSale.Model
{
    public class Product : ViewModelBase
    {
        CultureInfo svSE = CultureInfo.CreateSpecificCulture("sv-SE");
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public float Price { get; set; }
        public string PriceFormatted
        {
            get
            {
                return Price.ToString("0.00", svSE);
            }
        }

        private int amountSold;

        public int AmountSold
        {
            get { return amountSold; }
            set 
            { 
                amountSold = value;
                NotifyPropertyChanged();
            }
        }

        private int stock;

        public int Stock
        {
            get { return stock; }
            set 
            { 
                stock = value;
                NotifyPropertyChanged();
            }
        }


        public Brush Color{ get; set; }

        public Product(int id, string productName, string categoryName, float price, string colorString, int amountSold)
        {
            Id = id;
            Name = productName;
            Category = categoryName;
            Price = price;
            Color = (Brush) new BrushConverter().ConvertFromString(colorString);
            AmountSold = amountSold;
            Stock = 100;
        }
    }
}
