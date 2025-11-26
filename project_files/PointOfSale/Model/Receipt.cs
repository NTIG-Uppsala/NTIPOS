using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Model
{
    internal class Receipt
    {
        public string Time
        { get; set; }
        public int ID
        { get; set; }
        public int TotalSum
        { get; set; }

        public Receipt(string receiptTime, int receiptID, int ReceiptTotalSum)
        {
            Time = receiptTime;
            ID = receiptID;
            TotalSum = ReceiptTotalSum;
        }
    }
}
