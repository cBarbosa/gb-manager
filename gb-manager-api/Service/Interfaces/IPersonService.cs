using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using System;
using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IPersonService
    {
        Task<CommandResult> Create(CreatePersonCommand cmd);
        Task<CommandResult> Update(CreatePersonCommand cmd);
        Task<CommandResult> GetByLogin(string userName);
        Task<CommandResult> GetByOptions(string document, string name);
        Task<CommandResult> GetByRecordId(Guid recordId);
        Task<CommandResult> GetContracts(Guid recordId);
    }
}