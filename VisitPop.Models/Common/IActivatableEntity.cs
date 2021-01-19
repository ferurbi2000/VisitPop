namespace VisitPop.Domain.Common
{
    /// <summary>
    /// Interfaz que permite añadir funcionalidad a las entidades que si es activo o no
    /// </summary>
    public interface IActivatableEntity
    {
        public bool IsActive { get; set; }
    }
}
