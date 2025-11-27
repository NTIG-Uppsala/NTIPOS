using PointOfSale.Model;
using PointOfSale.ViewModel;
using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PointOfSale.ViewModel
{
    internal class ReceiptsViewModel : ViewModelBase
    {
        private static ReceiptsViewModel receiptsVm = new ReceiptsViewModel();

        public static ReceiptsViewModel ReceiptsVM { get { return receiptsVm; } }

        public ObservableCollection<Receipt> Receipts { get; set; }

        private Receipt selectedItem;
        public Receipt SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        private int currentID = 0;

        public ReceiptsViewModel()
        {
            Receipts = new ObservableCollection<Receipt> { };
        }

        public void AddReceipt(ObservableCollection<Article> articleCollection, float totalSumInt)
        {
            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

            List<ReceiptArticle> articleList = new List<ReceiptArticle>();

            foreach (Article article in articleCollection)
            {
                ReceiptArticle receiptArticle = new ReceiptArticle(article);
                articleList.Add(receiptArticle);
            }

            int receiptID = currentID;

            Receipts.Insert(0, new Receipt(articleList, formattedTime, receiptID, totalSumInt));

            currentID++;
        }
    }
}
