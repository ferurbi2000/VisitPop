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
    public class ContactStore : IContactStore
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public ContactStore()
        {
            //var task = Task.Run(async () => await GetFromAPI());
            //task.Wait();
            WebAPIUrl = "https://10.0.2.2:5001/api/People/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<IEnumerable<Person>> GetContactsAsync()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            List<Person> personList = new List<Person>();

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                try
                {
                    using (var response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            personList = JsonConvert.DeserializeObject<PageListPerson>(apiResponse).People;

                            return personList;
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

        public async Task DeleteContact(Person person)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + person.Id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        public async Task AddContact(Person person)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            Person receivedPerson = new Person();

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedPerson = JsonConvert.DeserializeObject<PagePerson>(apiResponse).Person;
                    }
                }
            }
        }

        public async Task UpdateContact(Person person)
        {

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            Person receivedPerson = new Person();

            using (var httpClient = new HttpClient(httpClientHandler))
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + person.Id, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedPerson = JsonConvert.DeserializeObject<Person>(apiResponse);
                    }
                }
            }
        }


        public async Task<Person> GetContact(int id)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            Person person = new Person();
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        person = JsonConvert.DeserializeObject<PagePerson>(apiResponse).Person;
                    }
                }
            }
            
            return await Task.FromResult(person);
        }
    }
}
