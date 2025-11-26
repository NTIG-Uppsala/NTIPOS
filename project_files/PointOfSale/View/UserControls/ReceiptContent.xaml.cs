using PointOfSale.ViewModel;
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

namespace PointOfSale.View.UserControls
{
    /// <summary>
    /// Interaction logic for ReceiptContent.xaml
    /// </summary>
    public partial class ReceiptContent : UserControl
    {
        public ReceiptContent()
        {
            InitializeComponent();
            DataContext = ReceiptsViewModel.ReceiptsVM;
        }
    }
}
