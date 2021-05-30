using gb_manager.Domain.Models;
using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IPersonService
    {
        Task<Person> GetByLogin(string userName);
    }
}