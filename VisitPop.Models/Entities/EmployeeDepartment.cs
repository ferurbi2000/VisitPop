﻿using Sieve.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;

namespace VisitPop.Domain.Entities
{
    [Table("EmployeeDepartment")]
    public class EmployeeDepartment : AuditableEntity
    {

        [Sieve(CanFilter = true, CanSort = true)]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
