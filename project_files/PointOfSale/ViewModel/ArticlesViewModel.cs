using PointOfSale.Model;
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
        public ObservableCollection<Article> Articles { get; set; }

        public ArticlesViewModel()
        {

        }
    }
}
