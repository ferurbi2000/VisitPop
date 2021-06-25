using System.Threading.Tasks;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.PersonType
{
    public interface IPersonTypeRepository
    {
        Task<PagingResponse<PersonTypeDto>> GetPersonTypesAsync(PersonTypeParametersDto personTypeParameters);
        Task<PersonTypeDto> GetPersonType(int id);
        Task<PersonTypeDto> AddPersonType(PersonTypeDto personType);
        Task UpdatePersonType(PersonTypeDto personType);
        Task DeletePersonType(PersonTypeDto personType);
    }
}
