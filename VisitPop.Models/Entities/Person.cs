using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("Person")]
    public class Person : AuditableEntity
    {

        [Sieve(CanFilter = true, CanSort = true)]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string DocId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PhoneNumber { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int PersonTypeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int CompanyId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_EMAIL_LENGTH)]
        public string EmailAddress { get; set; }


        [ForeignKey("PersonTypeId")]
        public PersonType PersonTypes { get; set; }

        [ForeignKey("CompanyId")]
        public Company Companies { get; set; }

        // add-on property marker - Do Not Delete This Comment

    }
}
