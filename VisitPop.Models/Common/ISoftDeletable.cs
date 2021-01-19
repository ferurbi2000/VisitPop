namespace VisitPop.Domain.Common
{
    /// <summary>
    /// Interfaz que permite añadir funcionalidad a las entidades para el borrado suave.
    /// Evita que se borre de la base de datos cualquier registro.
    /// </summary>
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
