using PointOfSale.ViewModel;
using PointOfSale.Model;
using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using static PointOfSale.MainWindow;

namespace PointOfSale.View.UserControls
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products
    {
        public Products()
        {
            InitializeComponent();
            DataContext = ProductsViewModel.ProductsVM;
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = (sender as FrameworkElement).DataContext as Product;
            ArticlesViewModel.ArticlesVM.AddProduct(product);
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
