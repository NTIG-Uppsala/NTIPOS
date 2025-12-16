using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PointOfSale.Model
{
    public class ProductCategory : ViewModelBase
    {
        public int Id
        { get; set; }

        public string Name
        { get; set; }

        public Brush Color
        { get; set; }

        private bool isShown;

        public bool IsShown
        {
            get { return isShown; }
            set 
            { 
                isShown = value;
                NotifyPropertyChanged();
            }
        }

        public ProductCategory(int id, string categoryName, string colorString)
        {
            Id = id;
            Name = categoryName;
            Color = (Brush) new BrushConverter().ConvertFromString(colorString);
            IsShown = false;
        }
    }
}