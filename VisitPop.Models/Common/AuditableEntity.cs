using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VisitPop.Domain.Common
{
    /// <summary>
    /// Clase abstracta para entregar a las entidades funcionalidad de ser auditado en la base de datos
    /// permite añadir registros de cuando fue creado, quien lo creo y quien y cuando lo modifico.
    /// </summary>
    public abstract class AuditableEntity : IIdentityEntity, IAuditableEntity, IActivatableEntity, ISoftDeletable
    {
        [Key]
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; }
        
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
