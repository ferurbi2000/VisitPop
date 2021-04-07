namespace VisitPop.Application.Dtos.Empleado
{
    public abstract class EmpleadoForManipulationDto
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Identidad { get; set; }
        public string Telefono { get; set; }
        public int DepartamentoEmpleadoId { get; set; }
        public string Email { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
