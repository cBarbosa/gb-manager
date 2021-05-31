using gb_manager.Domain.Shared.Command;
using gb_manager.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace gb_manager.Controller
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService service;

        public PersonController(IPersonService _service)
        {
            service = _service;
        }


        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] CreatePersonCommand cmd)
        {
            var result = await service.Create(cmd);

            return Ok(result);
        }

        [HttpGet("{document}")]
        public async Task<IActionResult> GetByDocument([FromRoute] string document)
        {
            var result = await service.GetByDocument(document);

            return Ok(result);
        }
    }
}
