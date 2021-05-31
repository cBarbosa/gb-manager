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
            var _person = await service.GetByLogin(auth.UserName);
            var _personData = (Domain.Models.Person)_person.Data;

            if (_personData == null)
                return new CommandResult(false, "Não autenticado", null);

            if (!_personData.IsPasswordValid(auth.Password))
                return new CommandResult(false, "Não autenticado", null);

            logger.LogInformation($"Autenticado com sucesso, {_personData.Email}");

            return new CommandResult(true, "Autenticado com sucesso", _personData);
        }
    }
}