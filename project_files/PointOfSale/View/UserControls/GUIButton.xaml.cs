using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for GUIButton.xaml
    /// </summary>
    public partial class GUIButton : UserControl, INotifyPropertyChanged
    {
        public GUIButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        private Brush color;
        private string label;
        private string title;
        private string price;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Brush Color
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged();
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                NotifyPropertyChanged();
            }
        }
        
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }

        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                NotifyPropertyChanged();
            }
        }

        public void NotifyPropertyChanged( [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event RoutedEventHandler Click
        {
            add { GUIbtn.AddHandler(ButtonBase.ClickEvent, value); }
            remove { GUIbtn.AddHandler(ButtonBase.ClickEvent, value); }
        }
    }
}
