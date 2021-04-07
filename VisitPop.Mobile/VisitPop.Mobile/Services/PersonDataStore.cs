using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;

namespace VisitPop.Mobile.Services
{
    public class PersonDataStore : IDataStore<Person>
    {
        private List<Person> people;

        public PersonDataStore()
        {
            //people = new List<Person>()
            //{
            //    new Person{Id= 1, Nombres = "Fernando", Apellidos="Urbina", DocIdentidad="001-040677"},
            //    new Person{Id= 2, Nombres = "Armando", Apellidos="Urbina", DocIdentidad="001-040677"},
            //    new Person{Id= 3, Nombres = "Mijail", Apellidos="Urbina", DocIdentidad="001-040677"},
            //    new Person{Id= 4, Nombres = "Maria Fernanda", Apellidos="Urbina", DocIdentidad="001-040677"}
            //};

            var task = Task.Run(async () => await GetFromAPI());
            task.Wait();

        }

        public async Task GetFromAPI()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            List<Person> personList = new List<Person>();

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                try
                {
                    using (var response = await httpClient.GetAsync("https://10.0.2.2:5001/api/People"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        people = JsonConvert.DeserializeObject<PageListPerson>(apiResponse).People;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
        }

        public async Task<bool> AddItemAsync(Person item)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            Person receivedPerson = new Person();

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://10.0.2.2:5001/api/People", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedPerson = JsonConvert.DeserializeObject<PagePerson>(apiResponse).Person;
                }
            }

            //people.Add(item);
            people.Add(receivedPerson);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Person item)
        {
            var oldItem = people.Where((Person arg) => arg.Id == item.Id).FirstOrDefault();
            people.Remove(oldItem);
            people.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = people.Where((Person arg) => arg.Id.ToString() == id).FirstOrDefault();
            people.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Person> GetItemAsync(string id)
        {
            return await Task.FromResult(people.FirstOrDefault(s => s.Id.ToString() == id));
        }
        

        public async Task<IEnumerable<Person>> GetItemsAsync(bool forceRefresh = false, bool forceAPI = false)
        {

            if (forceAPI)
            {
                var task = Task.Run(async () => await GetFromAPI());
                task.Wait();
            }


            //var httpClientHandler = new HttpClientHandler();
            //httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            //List<Person> personList = new List<Person>();
            //using (var httpClient = new HttpClient(httpClientHandler))
            //{
            //    try
            //    {
            //        using (var response = await httpClient.GetAsync("https://10.0.2.2:5001/api/People"))
            //        {
            //            string apiResponse = await response.Content.ReadAsStringAsync();
            //            people = JsonConvert.DeserializeObject<PageListPerson>(apiResponse).People;
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        Debug.WriteLine(ex.Message);
            //    }

            //}

            //return await Task.FromResult(people);
            return await Task.FromResult(people);
        }

        
    }
}
