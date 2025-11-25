using PointOfSale.MVVM;
using PointOfSale.Model;
using PointOfSale.View.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Navigation;

namespace PointOfSale.ViewModel
{
    public class ArticlesViewModel : ViewModelBase
    {
        private static ArticlesViewModel articlesVm = new ArticlesViewModel();
        public static ArticlesViewModel ArticlesVM { get { return articlesVm; } }
        public ObservableCollection<Article> Articles { get; set; }

        private int totalSum = 0;

        public int TotalSum
        {
            get { return totalSum; }
            set { 
                totalSum = value;
                NotifyPropertyChanged();
            }
        }


        public ArticlesViewModel()
        {
            Articles = new ObservableCollection<Article> { };
        }

        public void AddProduct(Product product)
        {
            Article relevantArticle;
            IEnumerable<Article> hej = Articles.Where(article => article.Product == product);
            if (!(hej != null && hej.Any()))
            {
                relevantArticle = new Article(product);
                Articles.Add(relevantArticle);
            }
            else
            {
                int relevantArticleIndex = Articles.IndexOf(hej.ElementAt(0));
                relevantArticle = Articles[relevantArticleIndex];
                relevantArticle.Quantity++;
                relevantArticle.Sum = relevantArticle.Quantity * relevantArticle.Product.Price;
                Articles.Remove(relevantArticle);
                Articles.Add(relevantArticle);
            }
            UpdateTotalSum();
        }

        public void UpdateTotalSum()
        {
            int totalSum = 0;
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
