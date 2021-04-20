using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitPop.MVC.Models
{
    public class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }

        [JsonProperty("Identidad")]
        public string Identidad { get; set; }

        //[JsonProperty("email")]
        //public string Email { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        //[JsonProperty("isActive")]
        //public bool IsActive { get; set; }

        //[JsonProperty("isDeleted")]
        //public bool IsDeleted { get; set; }
    }

    public class PageListPerson
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }

        [JsonProperty("data")]
        public List<Person> People { get; set; }
    }

    public class PagePerson
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }

        [JsonProperty("data")]
        public Person Person { get; set; }
    }
}
