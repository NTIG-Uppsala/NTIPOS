using PointOfSale.Model;
using PointOfSale.MVVM;
using PointOfSale.View.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace PointOfSale.ViewModel
{
    public class ArticlesViewModel : ViewModelBase
    {
        CultureInfo svSE = CultureInfo.CreateSpecificCulture("sv-SE");
        private static ArticlesViewModel articlesVm = new ArticlesViewModel();
        public static ArticlesViewModel ArticlesVM { get { return articlesVm; } }
        public ObservableCollection<Article> Articles { get; set; }

        private Article selectedItem;
        public Article SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        private float totalSum = 0;

        public float TotalSum
        {
            get { return totalSum; }
            set { 
                totalSum = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("TotalSumFormatted");
            }
        }
        public string TotalSumFormatted
        {
            get
            {
                return TotalSum.ToString("0.00", svSE);
            }
        }


        public ArticlesViewModel()
        {
            Articles = new ObservableCollection<Article> { };
        }

        public void AddProduct(Product product)
        {
            Article relevantArticle;
            IEnumerable<Article> wantedArticle = Articles.Where(article => article.Product == product);
            if (!(wantedArticle != null && wantedArticle.Any()))
            {
                relevantArticle = new Article(product);
                Articles.Add(relevantArticle);
            }
            else
            {
                int relevantArticleIndex = Articles.IndexOf(wantedArticle.ElementAt(0));
                relevantArticle = Articles[relevantArticleIndex];
                relevantArticle.Quantity++;
                relevantArticle.Sum = relevantArticle.Quantity * relevantArticle.Product.Price;
                Articles.Remove(relevantArticle);
                Articles.Add(relevantArticle);
            }
            SelectedItem = relevantArticle;
            UpdateTotalSum();
        }

        public void UpdateTotalSum()
        {
            float totalSum = 0;
            foreach (Article article in Articles)
            {
                totalSum += article.Product.Price * article.Quantity;
            }
            TotalSum = totalSum;
        }

        public void ClearBasket()
        {
            Articles.Clear();
            UpdateTotalSum();
        }
    }
}
