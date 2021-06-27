using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.MVC.Models;

namespace VisitPop.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //List<Personas> personList = new List<Personas>();
            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.GetAsync("https://localhost:5001/api/Personas"))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //        personList = JsonConvert.DeserializeObject<PageListPerson>(apiResponse).People;
            //    }
            //}
            return View();
        }

        public IActionResult Edit(int id)
        {

            //if (id != 0) //Persona ya existe?
            //{
            //    Personas person = new Personas();
            //    using (var httpClient = new HttpClient())
            //    {
            //        using (var response = await httpClient.GetAsync("https://localhost:5001/api/Personas/" + id))
            //        {
            //            string apiResponse = await response.Content.ReadAsStringAsync();
            //            person = JsonConvert.DeserializeObject<PagePerson>(apiResponse).Person;
            //        }
            //    }
            //    return View(person);
            //}

            return View();
        }

        [HttpPost]        
        public Task<IActionResult> Edit()
        {
            //Personas receivedPerson = new Personas();

            //if (person.Id == 0) // se trata que es un nuevo registro
            //{
            //    //Person receivedPerson = new Person();
            //    using (var httpClient = new HttpClient())
            //    {
            //        StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

            //        using (var response = await httpClient.PostAsync("https://localhost:5001/api/Personas", content))
            //        {
            //            string apiResponse = await response.Content.ReadAsStringAsync();
            //            receivedPerson = JsonConvert.DeserializeObject<PagePerson>(apiResponse).Person;
            //        }
            //    }
            //}
            //else // estamos editando un registro
            //{
            //    using (var httpClient = new HttpClient())
            //    {

            //        StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");                                        

            //        using (var response = await httpClient.PutAsync("https://localhost:5001/api/Personas/" + person.Id, content))
            //        {
            //            string apiResponse = await response.Content.ReadAsStringAsync();
            //            receivedPerson = JsonConvert.DeserializeObject<Personas>(apiResponse);
            //        }
            //    }
            //}


            //return RedirectToAction(nameof(Index));
            return null;
        }

        [HttpPost]
        public Task<IActionResult> Delete(int id)
        {
            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.DeleteAsync("https://localhost:5001/api/Personas/" + id))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //    }
            //}

            //return RedirectToAction(nameof(Index));
            return null;
        }
        
    }
}
