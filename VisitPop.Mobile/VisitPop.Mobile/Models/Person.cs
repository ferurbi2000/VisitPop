using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisitPop.Mobile.Models
{
    public class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }

        [JsonProperty("docIdentidad")]
        public string DocIdentidad { get; set; }

        [JsonProperty("telefono1")]
        public string Telefono1 { get; set; }
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
