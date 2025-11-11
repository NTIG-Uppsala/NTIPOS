using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointOfSale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EditCoffeeAmount(object sender, RoutedEventArgs e)
        {
            int change = 0;
            int amountInt = 0;

            if (!(sender == resetButton))
            {
                string changeString = $"{(sender as Button).Content}1";
                change = Convert.ToInt32(changeString);
                amountInt = Convert.ToInt32(amount.Text);
            }

            if (!(amountInt + change < 0))
            {
                int newAmountInt = amountInt + change;
                amount.Text = Convert.ToString(newAmountInt);
                UpdateTotalSum(newAmountInt);
            }
        }

        private void UpdateTotalSum(int newAmountInt)
        {
            int singlePrice = Convert.ToInt32(price.Text);
            int totalPrice = singlePrice * newAmountInt;
            totalSum.Text = Convert.ToString(totalPrice);
        }
    }
}