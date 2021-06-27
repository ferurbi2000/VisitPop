using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Person
{
    public interface IPersonRepository
    {
        Task<PagingResponse<PersonDto>> GetPersonsAsync(PersonParametersDto personParameters);
        Task<PersonDto> GetPerson(int id);
        Task<PersonDto> AddPerson(PersonDto person);
        Task UpdatePerson(PersonDto person);
        Task DeletePerson(PersonDto person);

        Task<PagingResponse<PersonTypeDto>> GetPersonTypesAsync(PersonTypeParametersDto personTypeParameters);
        Task<PagingResponse<CompanyDto>> GetCompaniesAsync(CompanyParametersDto companyParameters);
    }
}
