using System.Collections.Generic;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;

namespace VisitPop.Mobile.Services
{
    public interface IContactStore
    {
        Task<IEnumerable<Person>> GetContactsAsync();
        Task<Person> GetContact(int id);
        Task AddContact(Person person);
        Task UpdateContact(Person person);
        Task DeleteContact(Person person);
    }
}
