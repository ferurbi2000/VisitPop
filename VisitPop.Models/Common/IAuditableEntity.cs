using System;

namespace VisitPop.Domain.Common
{
    /// <summary>
    /// Interfaz que permite añadir campos auditables como firmas a las clases que lo invoquen
    /// </summary>
    public interface IAuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
