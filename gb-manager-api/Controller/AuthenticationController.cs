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
        public async Task<IActionResult> Post(
            [FromBody] AuthenticationCommand authentication)
        {
            var result = await service.Authenticate(authentication);

            if (!result.Success)
                return Unauthorized(result);

            var _person = (Person)result.Data;
            _person.Password = string.Empty;

            var AccessToken = BindTokenToResult(_person);

            if (!AccessToken.Success)
                return Unauthorized(result);

            return Ok(new CommandResult(true, result.Message, new
            {
                AccessToken = AccessToken.Data,
                Person = _person
            }));
        }

        private static CommandResult BindTokenToResult(
            Person data)
        {
            var accessToken = TokenService.GenerateToken(
                data.Id.ToString(),
                data.Email,
                data.Profile);

            return new CommandResult(accessToken!=null,
                (accessToken!=null
                    ? Messages.SUCCESS_AUTHENTICATION
                    : Messages.ERROR_AUTHENTICATION)
                , accessToken);
        }
    }
}