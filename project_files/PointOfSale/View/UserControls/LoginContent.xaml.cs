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
using PointOfSale.ViewModel;

namespace PointOfSale.View.UserControls
{
    /// <summary>
    /// Interaction logic for LoginContent.xaml
    /// </summary>
    public partial class LoginContent : UserControl 
    {
        public LoginContent()
        {
            InitializeComponent();
            DataContext = LoginViewModel.LoginVM;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userID = UserIDInput.Text;
            UserIDInput.Clear();
            UserPasswordInput.Clear();
            LoginViewModel.LoginVM.Login(userID);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel.LoginVM.Logout();
        }

        private void SetAPIURLButton_Click(object sender, RoutedEventArgs e)
        {
            if (APIURLInput.Text != "")
            {
                string APIURL = APIURLInput.Text;
                APIURLInput.Clear();
                LoginViewModel.LoginVM.SetAPIURL(APIURL);
            }
        }
    }
}
