using PointOfSale.MVVM;
using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PointOfSale.ViewModel
{
    public class ButtonPanelViewModel : ViewModelBase
    {
        private static ButtonPanelViewModel buttonPanelVm = new ButtonPanelViewModel();
        public static ButtonPanelViewModel ButtonPanelVM { get { return buttonPanelVm; } }

        private bool isLoggedIn;

        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set 
            { 
                isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }

        public void Abort()
        {
            ArticlesViewModel.ArticlesVM.ClearBasket();
        }

        public void Checkout()
        {
            if (ArticlesViewModel.ArticlesVM.Articles.Any())
            {
                float TotalSum = ArticlesViewModel.ArticlesVM.TotalSum;
                ObservableCollection<Article> ArticleCollection = ArticlesViewModel.ArticlesVM.Articles;
                ReceiptsViewModel.ReceiptsVM.AddReceipt(ArticleCollection, TotalSum);
                ReceiptsViewModel.ReceiptsVM.PrintReceipt(ReceiptsViewModel.ReceiptsVM.Receipts[0]);
                DatabaseHelper.AddAmountSold(ArticleCollection);
                ArticlesViewModel.ArticlesVM.ClearBasket();
            }
        }
    }
}
