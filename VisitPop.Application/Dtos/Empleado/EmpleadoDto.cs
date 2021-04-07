using VisitPop.Application.Dtos.DepartamentoEmpleado;

namespace VisitPop.Application.Dtos.Empleado
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Identidad { get; set; }
        public string Telefono { get; set; }
        public int DepartamentoEmpleadoId { get; set; }
        public string Email { get; set; }
        public DepartamentoEmpleadoDto DepartamentoEmpleado { get; set; }
        
        // add-on property marker - Do Not Delete This Comment
    }
}
