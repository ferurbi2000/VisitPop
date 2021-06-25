namespace VisitPop.Application.Interfaces.PersonType
{
    using Domain.Entities;
    using global::VisitPop.Application.Dtos.PersonType;
    using global::VisitPop.Application.Wrappers;
    using System.Threading.Tasks;

    public interface IPersonTypeRepository
    {
        Task<PagedList<PersonType>> GetPersonTypes(PersonTypeParametersDto personTypeParameters);
        Task<PersonType> GetPersonTypeAsync(int id);
        PersonType GetPersonType(int id);
        Task AddPersonType(PersonType personType);
        void DeletePersonType(PersonType personType);
        void UpdatePersonType(PersonType personType);
        bool Save();
        Task<bool> SaveAsync();
    }
}
