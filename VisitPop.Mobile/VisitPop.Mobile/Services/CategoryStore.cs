using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;

namespace VisitPop.Mobile.Services
{
    public class CategoryStore : ICategoryStore
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public CategoryStore()
        {
            WebAPIUrl = "https://10.0.2.2:5001/api/Categories/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            List<Category> categoryList = new List<Category>();

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                try
                {
                    using (var response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            categoryList = JsonConvert.DeserializeObject<PageListCategory>(apiResponse).Categories;

                            return categoryList;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                }
                return null;
            }
        }

        public async Task DeleteCategory(Category category)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + category.Id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        public async Task AddCategory(Category category)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            Category receivedCategory = new Category();

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedCategory = JsonConvert.DeserializeObject<PageCategory>(apiResponse).Category;
                    }
                }
            }
        }

        public async Task UpdateCategory(Category category)
        {

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            Category receivedCategory = new Category();

            using (var httpClient = new HttpClient(httpClientHandler))
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + category.Id, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedCategory = JsonConvert.DeserializeObject<Category>(apiResponse);
                    }
                }
            }
        }


        public async Task<Category> GetCategory(int id)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            Category category = new Category();
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        category = JsonConvert.DeserializeObject<PageCategory>(apiResponse).Category;
                    }
                }
            }

            return await Task.FromResult(category);
        }
    }
}
