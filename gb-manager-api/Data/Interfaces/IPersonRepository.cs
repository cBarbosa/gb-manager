using gb_manager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetByLogin(string userName);
        Task<IEnumerable<Person>> GetByDocument(string document);
    }
}