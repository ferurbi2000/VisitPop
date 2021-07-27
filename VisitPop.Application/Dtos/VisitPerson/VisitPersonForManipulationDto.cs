using System;

namespace VisitPop.Application.Dtos.VisitPerson
{
    public abstract class VisitPersonForManipulationDto
    {
        public int VisitId { get; set; }
        public int PersonId { get; set; }
        public int VehicleTypeId { get; set; }
        public string Plate { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }


        // add-on property marker - Do Not Delete This Comment
    }
}
