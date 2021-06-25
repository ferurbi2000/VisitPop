using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.VehicleType
{
    public class VehicleTypeDto: AuditableEntity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage ="You must enter the Vehicle Type Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name ="Vehicle Type")]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
