using System;
using System.ComponentModel.DataAnnotations;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.VisitPerson
{
    public class VisitPersonDto: AuditableEntity
    {
        [Required(ErrorMessage = "You must select a Visit Ticket")]
        public int? VisitId { get; set; }

        [Required(ErrorMessage = "You must select a Person")]
        public int? PersonId { get; set; }

        [Required(ErrorMessage = "You must select a Vehicle Type")]
        public int? VehicleTypeId { get; set; }

        [StringLength(VisitEntityConstants.MAX_PHONE_LENGTH)]
        public string Plate { get; set; }

        [Display(Name = "Date In")]
        public DateTime? DateIn { get; set; }

        [Display(Name = "Date Out")]
        public DateTime? DateOut { get; set; }

        public VisitDto Visit { get; set; }

        public PersonDto Person { get; set; }

        public VehicleTypeDto VehicleType { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
