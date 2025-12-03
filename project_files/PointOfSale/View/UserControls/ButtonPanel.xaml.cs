using PointOfSale.ViewModel;
using PointOfSale.Model;
using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PointOfSale.View.UserControls
{
    /// <summary>
    /// Interaction logic for ButtonPanel.xaml
    /// </summary>
    public partial class ButtonPanel : UserControl
    {
        public ButtonPanel()
        {
            InitializeComponent();
        }

        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            ArticlesViewModel.ArticlesVM.ClearBasket();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (ArticlesViewModel.ArticlesVM.Articles.Any())
            {
                float TotalSum = ArticlesViewModel.ArticlesVM.TotalSum;
                ObservableCollection<Article> ArticleCollection = ArticlesViewModel.ArticlesVM.Articles;
                ReceiptsViewModel.ReceiptsVM.AddReceipt(ArticleCollection, TotalSum);
                ReceiptsViewModel.ReceiptsVM.PrintReceipt(ReceiptsViewModel.ReceiptsVM.Receipts[0]);
                DatabaseHelper.AddAmountSold(ArticleCollection);
            }
            ArticlesViewModel.ArticlesVM.ClearBasket();
        }
    }
}
