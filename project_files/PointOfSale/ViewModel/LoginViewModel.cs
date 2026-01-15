using PointOfSale.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PointOfSale.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private static LoginViewModel loginVm = new LoginViewModel();
        public static LoginViewModel LoginVM { get { return loginVm; } }

        private string apiUrlFileLocationString = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/POS/configuration.txt";

        private string apiUrl;

        public string ApiUrl
        {
            get { return apiUrl; }
            set 
            { 
                apiUrl = value;
                NotifyPropertyChanged();
            }
        }

        private bool isLoggedIn;

        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set 
            { 
                isLoggedIn = value;
                ButtonPanelViewModel.ButtonPanelVM.IsLoggedIn = value;
                ProductsViewModel.ProductsVM.IsLoggedIn = value;
                NotifyPropertyChanged();
            }
        }


        private string loginStatusMessage;

        public string LoginStatusMessage
        {
            get { return loginStatusMessage; }
            set
            {
                loginStatusMessage = value;
                NotifyPropertyChanged();
            }
        }

        const string LOGGED_OUT_STATUS_MESSAGE = "Logga in";
        const string LOGGED_IN_STATUS_MESSAGE = "Inloggad som ";

        public LoginViewModel()
        {
            LoginStatusMessage = LOGGED_OUT_STATUS_MESSAGE;
            using (StreamReader reader = new StreamReader(apiUrlFileLocationString))
            {
                ApiUrl = reader.ReadLine();
            }
        }

        public void Login(string userID)
        {
            if (userID != "")
            {
                LoginStatusMessage = LOGGED_IN_STATUS_MESSAGE + userID;
                IsLoggedIn = true;
            }
        }

        public void Logout()
        {
            LoginStatusMessage = LOGGED_OUT_STATUS_MESSAGE;
            IsLoggedIn = false;
        }

        public void SetAPIURL(string APIURL)
        {
            ApiUrl = APIURL;
            using (StreamWriter writer = new StreamWriter(apiUrlFileLocationString, false))
            {
                writer.WriteLine(ApiUrl);
            }
        }
    }
}
