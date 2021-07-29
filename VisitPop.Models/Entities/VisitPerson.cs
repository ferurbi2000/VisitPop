using Sieve.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("VisitPerson")]
    public class VisitPerson : AuditableEntity
    {
        
                
        [Sieve(CanFilter = true, CanSort = false)]
        public int PersonId { get; set; }

        [Sieve(CanFilter = true, CanSort = false)]
        public int VisitId { get; set; }

        [Sieve(CanFilter = true, CanSort = false)]
        public int VehicleTypeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]        
        public string Plate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? DateIn { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? DateOut { get; set; }

        [ForeignKey("VisitId")]
        public Visit Visit { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [ForeignKey("VehicleTypeId")]
        public VehicleType VehicleType { get; set; }

        // add-on property marker - Do Not Delete This Comment

        //Pertenencias

    }
}
