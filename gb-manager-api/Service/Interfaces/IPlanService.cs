using gb_manager.Domain.Shared;
using System;
using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IPlanService
    {
        Task<CommandResult> GetActives();
        Task<CommandResult> GetByRecordId(Guid recordId);
    }
}