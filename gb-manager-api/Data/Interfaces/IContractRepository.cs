using gb_manager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data.Interfaces
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetByPersonId(int personId);
        Task<Contract> GetByRecordId(Guid recordId);
    }
}