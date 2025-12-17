using PointOfSale.ViewModel;
using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
using PointOfSale.MVVM;

namespace PointOfSale.View.UserControls
{
    /// <summary>
    /// Interaction logic for StockContent.xaml
    /// </summary>
    public partial class StockContent : UserControl
    {
        public StockContent()
        {
            InitializeComponent();
            DataContext = ProductsViewModel.ProductsVM;
        }

        private void UpdateStockButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Product product in ProductsViewModel.ProductsVM.Products)
            {
                product.Stock -= product.AmountSold;
                DatabaseHelper.ResetAmountSold(product);
            }
        }
    }
}
