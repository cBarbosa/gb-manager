using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IContractService
    {
        Task<CommandResult> Create(CreateContractCommand cmd);
        Task<CommandResult> Update(CreateContractCommand cmd);
    }
}