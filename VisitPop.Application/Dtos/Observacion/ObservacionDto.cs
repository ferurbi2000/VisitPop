using VisitPop.Application.Dtos.Visita;

namespace VisitPop.Application.Dtos.Observacion
{
    public class ObservacionDto
    {
        public int Id { get; set; }
        public int VisitaId { get; set; }
        public string Nota { get; set; }

        //public VisitaDto Visita { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
