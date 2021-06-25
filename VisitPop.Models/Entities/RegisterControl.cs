using Sieve.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;

namespace VisitPop.Domain.Entities
{
    [Table("RegisterControl")]
    public class RegisterControl : AuditableEntity
    {

        [Sieve(CanFilter = true, CanSort = true)]
        public string Name { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Location { get; set; }

        public string Description { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
