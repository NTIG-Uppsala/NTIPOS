using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointOfSale.View.UserControls
{
    /// <summary>
    /// Interaction logic for CategoryButton.xaml
    /// </summary>
    public partial class CategoryButton : UserControl, INotifyPropertyChanged
    {
        public CategoryButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private string categoryName;

        public string CategoryName
        {
            get { return categoryName; }
            set { 
                categoryName = value;
                OnPropertyChanged();
            }
        }

        public string CategoryKey { get; set; }

        private Brush categoryColor;

        public Brush CategoryColor
        {
            get { return categoryColor; }
            set { 
                categoryColor = value;
                OnPropertyChanged("CategoryColor");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void categoryButton_Click(object sender, RoutedEventArgs e)
        {
            ListOfProducts listOfProducts = (ListOfProducts) Products.ProductsClass.productsGrid.FindName(CategoryKey);
            listOfProducts.Visibility = Visibility.Visible;
        }
    }
}
