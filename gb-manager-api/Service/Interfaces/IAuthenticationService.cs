using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<CommandResult> Authenticate(AuthenticationCommand auth);
    }
}