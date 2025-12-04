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
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Globalization;

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
            string formattedTime = currentTime.ToString("yyyy-MM-dd HH.mm.ss");

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

        public void PrintReceipt(Receipt receipt)
        {
            CultureInfo.CurrentCulture = new CultureInfo("sv-SE");

            string destination = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/POS/receipts/" + receipt.ID + " printed at " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + " issued at " + receipt.Time + ".pdf";
            FileInfo file = new FileInfo(destination);
            file.Directory.Create();

            var writer = new PdfWriter(destination);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            Style header = new Style()
                .SetFontSize(45)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            Style cell = new Style()
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            Style headerCell = new Style()
                .SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER)
                .SetBorderRight(iText.Layout.Borders.Border.NO_BORDER)
                .SetBorderTop(iText.Layout.Borders.Border.NO_BORDER);

            Style sumRowCell = new Style()
                .SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER)
                .SetBorderRight(iText.Layout.Borders.Border.NO_BORDER)
                .SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER);

            // Header
            document.Add(new Paragraph("Benny & Gänget").AddStyle(header));

            // Info
            document.Add(new Paragraph()
                    .Add("Kioskvägen 1\n")
                    .Add("753 28 Uppsala\n")
                    .Add("Org.nr: 123456-7890\n")
                    .Add("Kassa 1\n")
                    .Add(receipt.Time)
                    .Add("\nKvitto nr: ")
                    .Add(receipt.ID.ToString())
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    );

            // Articles
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 6, 1, 1, 2 }))
                .SetWidth(UnitValue.CreatePercentValue(90));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Artikel")).AddStyle(headerCell));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Antal")).AddStyle(headerCell));
            table.AddHeaderCell(new Cell().Add(new Paragraph("A-pris")).AddStyle(headerCell));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Total")).AddStyle(headerCell));

            foreach(ReceiptArticle article in receipt.ArticleList)
            {
                table.AddCell(new Cell().Add(new Paragraph(article.Product.Name)).AddStyle(cell));
                table.AddCell(new Cell().Add(new Paragraph(article.Quantity.ToString())).AddStyle(cell));
                table.AddCell(new Cell().Add(new Paragraph(article.Product.PriceFormatted)).AddStyle(cell));
                table.AddCell(new Cell().Add(new Paragraph(article.SumFormatted)).AddStyle(cell));
            }

            // Sum
            table.AddCell(new Cell().Add(new Paragraph("Att betala:")).AddStyle(sumRowCell));
            table.AddCell(new Cell().Add(new Paragraph("")).AddStyle(sumRowCell));
            table.AddCell(new Cell().Add(new Paragraph("")).AddStyle(sumRowCell));
            table.AddCell(new Cell()
                    .Add(new Paragraph(receipt.TotalSumFormatted)
                    .Add(" kr"))
                    .AddStyle(sumRowCell));

            // VAT-Info
            table.AddCell(new Cell().Add(new Paragraph("Varav moms (25%): ")).AddStyle(cell));
            table.AddCell(new Cell().Add(new Paragraph("")).AddStyle(cell));
            table.AddCell(new Cell().Add(new Paragraph("")).AddStyle(cell));
            table.AddCell(new Cell().Add(new Paragraph()
                    .Add((receipt.TotalSum * 0.2).ToString("#.00"))
                    .Add(" kr")
                        ).AddStyle(cell));

            document.Add(table);

            document.Add(new Paragraph("Betalsätt: Kontanter"));

            document.Add(new Paragraph("Ha en bra dag!")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(30));

            document.Close();
        }
    }
}
