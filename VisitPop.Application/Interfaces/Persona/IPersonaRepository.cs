namespace VisitPop.Application.Interfaces.Persona
{
    using Application.Dtos.Persona;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IPersonaRepository
    {
        Task<PagedList<Persona>> GetPersonasAsync(PersonaParametersDto personaParameters);
        Task<Persona> GetPersonaAsync(int id);
        Persona GetPersona(int id);
        Task AddPersona(Persona persona);
        void DeletePersona(Persona persona);
        void UpdatePersona(Persona persona);
        bool Save();
        Task<bool> SaveAsync();
    }
}
