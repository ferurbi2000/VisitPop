using VisitPop.Application.Dtos.Empresa;
using VisitPop.Application.Dtos.TipoPersona;

namespace VisitPop.Application.Dtos.Persona
{
    public class PersonaDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Identidad { get; set; }
        public string Telefono { get; set; }
        public int TipoPersonaId { get; set; }
        public int EmpresaId { get; set; }
        public string Email { get; set; }
        public TipoPersonaDto TipoPersona { get; set; }
        public EmpresaDto Empresa { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
