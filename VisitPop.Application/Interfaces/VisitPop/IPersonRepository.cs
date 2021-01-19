using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Interfaces.VisitPop
{
    public interface IPersonRepository
    {
        Task<PagedList<Person>> GetPersonsAsync(PersonParametersDto personParameters);
        Task<Person> GetPersonAsync(int id);
        Person GetPerson(int id);
        Task AddPerson(Person person);
        void DeletePerson(Person person);
        void UpdatePerson(Person person);
        bool Save();
        Task<bool> SaveAsync();
    }
}
