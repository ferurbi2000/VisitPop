namespace VisitPop.Application.Interfaces.Person
{
    using Application.Dtos.Person;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

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
