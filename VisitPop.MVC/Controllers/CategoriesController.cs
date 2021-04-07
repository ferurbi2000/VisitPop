using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.MVC.Models;

namespace VisitPop.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Category> categories = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:5001/api/Categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<PageListCategory>(apiResponse).Categories;
                }
            }
            return View(categories);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id != 0) //Category ya existe?
            {
                Category category = new Category();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:5001/api/Categories/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        category = JsonConvert.DeserializeObject<PageCategory>(apiResponse).Category;
                    }
                }
                return View(category);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            Category receivedCategory = new Category();

            if (category.Id == 0) // se trata que es un nuevo registro
            {
                //Person receivedPerson = new Person();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:5001/api/Categories", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedCategory = JsonConvert.DeserializeObject<PageCategory>(apiResponse).Category;
                    }
                }
            }
            else // estamos editando un registro
            {
                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync("https://localhost:5001/api/Categories/" + category.Id, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedCategory = JsonConvert.DeserializeObject<Category>(apiResponse);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:5001/api/Categories/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
