﻿using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("TipoPersona")]
    public class TipoPersona : AuditableEntity
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        public string Nombre { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
