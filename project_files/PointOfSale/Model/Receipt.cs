using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    internal class Receipt
    {
        public List<ReceiptArticle> ArticleList
        { get; set; }
        public string Time
        { get; set; }
        public int ID
        { get; set; }
        public int TotalSum
        { get; set; }
        public double VAT
        { get; set; }

        public Receipt(List<ReceiptArticle> receiptArticleList, string receiptTime, int receiptID, int ReceiptTotalSum)
        {
            ArticleList = receiptArticleList;
            Time = receiptTime;
            ID = receiptID;
            TotalSum = ReceiptTotalSum;
            VAT = TotalSum * 0.25;
        }
    }
}
