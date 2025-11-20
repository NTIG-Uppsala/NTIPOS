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
    /// Interaction logic for ListOfProducts.xaml
    /// </summary>
    public partial class ListOfProducts : UserControl
    {
        public ListOfProducts()
        {
            InitializeComponent();
            foreach (Products.Product product in CategoryProductsList)
            {
                GUIButton productButton = new()
                {
                    Title = product.Name,
                    Price = $"{product.Price}kr",
                    Color = CategoryColor
                };
            }
        }

        public List<Products.Product> CategoryProductsList { get; set; }
        public Brush CategoryColor { get; set; }
    }
}
