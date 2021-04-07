using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitPop.Application.Dtos.Person
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string DocIdentidad { get; set; }
        public string Telefono1 { get; set; }
        //public string Telefono2 { get; set; }
        public string Email { get; set; }
        //public string Observacion { get; set; }

        // add-on property marker - Do Not Delete This Comment

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
