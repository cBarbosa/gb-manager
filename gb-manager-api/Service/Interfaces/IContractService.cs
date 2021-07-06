using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using System;
using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IContractService
    {
        Task<CommandResult> Create(CreateContractCommand cmd);
        Task<CommandResult> Update(CreateContractCommand cmd);
        Task<CommandResult> AddPerson(Guid contractId, Guid personId);
        Task<CommandResult> GetByRecordId(Guid recordId);
        Task<CommandResult> GetByPersonRecordId(Guid personRecordId);
        Task<CommandResult> GetInstallmentsByRecordId(Guid recordId);
    }
}