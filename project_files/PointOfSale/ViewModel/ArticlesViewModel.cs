using PointOfSale.Model;
using PointOfSale.View.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.ViewModel
{
    public class ArticlesViewModel
    {
        public static ObservableCollection<Article> Articles { get; set; }

        public ArticlesViewModel()
        {
            Articles = new ObservableCollection<Article> { };
        }

        public void AddProduct(Product product)
        {
            Articles.Add(new Article(product));
        }
    }
}
