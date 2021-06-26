using gb_manager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetActives();
        Task<Plan> GetByRecordId(Guid recordId);
    }
}