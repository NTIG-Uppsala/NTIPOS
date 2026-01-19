using System.Net.Http;
using System.Net.Http.Json;
using PointOfSale.Model;
using PointOfSale.ViewModel;

namespace PointOfSale.MVVM
{
    class APIHelper
    {
        static readonly HttpClient client = new HttpClient();

        public record class APIProduct(
                int? id = null,
                string? name = null,
                int? category = null,
                float? price = null,
                int? stock = null);
        
        public record class APICategory(
                int? id = null,
                string? name = null,
                string? color = null);

        public static async Task FetchData()
        {
            try
            {
                var products = await client.GetFromJsonAsync<List<APIProduct>>($"{LoginViewModel.LoginVM.ApiUrl}/products");
                var categories = await client.GetFromJsonAsync<List<APICategory>>($"{LoginViewModel.LoginVM.ApiUrl}/categories");
                if (products == null) {throw new Exception("Products not found");};
                if (categories == null) {throw new Exception("Categories not found");};

                DatabaseHelper.UpdateCategories(categories);
                DatabaseHelper.UpdateProducts(products);

                ProductsViewModel.ProductsVM.getAllCategories(DatabaseHelper.connectionString);
                ProductsViewModel.ProductsVM.getAllProducts(DatabaseHelper.connectionString);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine("\nException Caught!");
                System.Diagnostics.Debug.WriteLine("Message : {0} ", e.Message);
            }
        }
    }
}
