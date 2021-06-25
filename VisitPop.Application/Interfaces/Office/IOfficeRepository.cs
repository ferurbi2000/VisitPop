namespace VisitPop.Application.Interfaces.Office
{
    using Application.Dtos.Office;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IOfficeRepository
    {
        Task<PagedList<Office>> GetOfficesAsync(OfficeParametersDto officeParametersDto);
        Task<Office> GetOfficeAsync(int Id);
        Office GetOffice(int Id);
        Task AddOffice(Office office);
        void DeleteOffice(Office office);
        void UpdateOffice(Office office);
        bool Save();
        Task<bool> SaveAsync();
    }
}
