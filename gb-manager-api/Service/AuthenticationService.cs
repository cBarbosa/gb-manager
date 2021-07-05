using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using gb_manager.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace gb_manager.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> logger;
        private readonly IPersonService service;

        public AuthenticationService(
            ILogger<AuthenticationService> _logger
            , IPersonService _service)
        {
            logger = _logger;
            service = _service;
        }

        public async Task<CommandResult> Authenticate(AuthenticationCommand auth)
        {
            var _personData = (Domain.Models.Person)(await service.GetByLogin(auth.UserName)).Data;

            if (_personData == null)
            {
                logger.LogError($"{Messages.ERROR_USER_NOT_EXISTS} {auth.UserName}");
                return new CommandResult(false, Messages.ERROR_USER_NOT_EXISTS, auth.UserName);
            }

            if (_personData.IsPasswordValid(auth.Password))
            {
                logger.LogInformation($"{Messages.SUCCESS_AUTHENTICATION} {_personData.Email}");
                return new CommandResult(true, Messages.SUCCESS_AUTHENTICATION, _personData);
            }

            logger.LogError($"{Messages.ERROR_AUTHENTICATION} {_personData.Email}");
            return new CommandResult(false, Messages.ERROR_AUTHENTICATION, null);
        }
    }
}