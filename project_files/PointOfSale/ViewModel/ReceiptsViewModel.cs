using PointOfSale.Model;
using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.ViewModel
{
    internal class ReceiptsViewModel : ViewModelBase
    {
        private static ReceiptsViewModel receiptsVm = new ReceiptsViewModel();

        public static ReceiptsViewModel ReceiptsVM { get { return receiptsVm; } }

        public ObservableCollection<Receipt> Receipts { get; set; }

        private int currentID = 0;

        public ReceiptsViewModel()
        {
            Receipts = new ObservableCollection<Receipt> { };
        }

        public void AddReceipt(int TotalSumInt)
        {
            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

            int receiptID = currentID;

            Receipts.Add(new Receipt(formattedTime, receiptID, TotalSumInt));

            currentID++;
        }
    }
}
