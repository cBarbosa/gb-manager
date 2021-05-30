using gb_manager.Domain.Models;
using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using gb_manager.Service.Interfaces;
using gb_manager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace gb_manager.Controller
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService service;

        public AuthenticationController(IAuthenticationService _service)
        {
            service = _service;
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] AuthenticationCommand authentication)
        {
            var result = await service.Authenticate(authentication);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(BindTokenToResult(result));
        }

        private static CommandResult BindTokenToResult(
            CommandResult tokenResult)
        {
            var data = (Person)tokenResult.Data;

            var accessToken = TokenService.GenerateToken(data.Id.ToString(), data.Email, data.Profile);

            return new CommandResult(true, "Autenticado com sucesso", new { accessToken });
        }
    }
}