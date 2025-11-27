using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    internal class Receipt
    {
        CultureInfo svSE = CultureInfo.CreateSpecificCulture("sv-SE");
        public List<ReceiptArticle> ArticleList
        { get; set; }
        public string Time
        { get; set; }
        public int ID
        { get; set; }

        public float TotalSum { get; set; }
        public string TotalSumFormatted
        {
            get
            {
                return TotalSum.ToString("0.00", svSE);
            }
        }

        public float VAT { get; set; }
        public string VATFormatted
        {
            get
            {
                return VAT.ToString("0.00", svSE);
            }
        }

        public Receipt(List<ReceiptArticle> receiptArticleList, string receiptTime, int receiptID, float ReceiptTotalSum)
        {
            ArticleList = receiptArticleList;
            Time = receiptTime;
            ID = receiptID;
            TotalSum = ReceiptTotalSum;
            VAT = (float)(TotalSum * 0.25);
        }
    }
}
