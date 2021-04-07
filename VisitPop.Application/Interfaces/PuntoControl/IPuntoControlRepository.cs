namespace VisitPop.Application.Interfaces.PuntoControl
{
    using Application.Dtos.PuntoControl;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IPuntoControlRepository
    {
        Task<PagedList<PuntoControl>> GetPuntoControlesAsync(PuntoControlParametersDto puntoControlParameters);
        Task<PuntoControl> GetPuntoControlAsync(int id);
        PuntoControl GetPuntoControl(int id);
        Task AddPuntoControl(PuntoControl puntoControl);
        void DeletePuntoControl(PuntoControl puntoControl);
        void UpdatePuntoControl(PuntoControl puntoControl);
        bool Saves();
        Task<bool> SaveAsync();
    }
}
