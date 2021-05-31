using gb_manager.Domain.Shared;
using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IPlanService
    {
        Task<CommandResult> GetActive();
    }
}