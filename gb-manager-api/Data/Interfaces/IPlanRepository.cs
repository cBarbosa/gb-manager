using gb_manager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetActive();
    }
}