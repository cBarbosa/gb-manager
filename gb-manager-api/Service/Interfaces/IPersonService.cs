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
        Task<CommandResult> GetByDocument(string document);
        Task<CommandResult> GetByRecordId(Guid recordId);
    }
}