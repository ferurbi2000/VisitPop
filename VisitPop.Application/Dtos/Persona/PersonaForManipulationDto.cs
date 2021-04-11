namespace VisitPop.Application.Dtos.Persona
{
    public abstract class PersonaForManipulationDto
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Identidad { get; set; }
        public string Telefono { get; set; }
        public int TipoPersonaId { get; set; }
        public int EmpresaId { get; set; }
        public string Email { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
