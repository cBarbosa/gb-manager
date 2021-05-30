using gb_manager.Domain.Models;
using System.Threading.Tasks;

namespace gb_manager.Data.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetByLogin(string userName);
    }
}