using gb_manager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data.Interfaces
{
    public interface IContractRepository
    {
        Task<Contract> Create(Contract contract);
        Task<IEnumerable<Contract>> GetByPersonId(int id);
    }
}