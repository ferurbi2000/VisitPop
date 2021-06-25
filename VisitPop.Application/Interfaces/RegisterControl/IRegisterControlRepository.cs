namespace VisitPop.Application.Interfaces.RegisterControl
{
    using Application.Dtos.RegisterControl;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IRegisterControlRepository
    {
        Task<PagedList<RegisterControl>> GetRegisterControlsAsync(RegisterControlParametersDto registerControlParameters);
        Task<RegisterControl> GetRegisterControlAsync(int id);
        RegisterControl GetRegisterControl(int id);
        Task AddRegisterControl(RegisterControl registerControl);
        void DeleteRegisterControl(RegisterControl registerControl);
        void UpdateRegisterControl(RegisterControl registerControl);
        bool Saves();
        Task<bool> SaveAsync();
    }
}
