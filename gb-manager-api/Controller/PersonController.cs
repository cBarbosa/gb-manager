using gb_manager.Domain.Shared.Command;
using gb_manager.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPut("")]
        public async Task<IActionResult> Put([FromBody] CreatePersonCommand cmd)
        {
            var result = await service.Create(cmd);

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Post([FromBody] CreatePersonCommand cmd)
        {
            var result = await service.Create(cmd);

            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(
            [FromQuery] string document,
            [FromQuery] string name)
        {
            var result = await service.GetByOptions(document, name);

            return Ok(result);
        }

        [HttpGet("{recordId}/contracts")]
        public async Task<IActionResult> GetContracts(
            [FromRoute] Guid recordId)
        {
            var result = await service.GetContracts(recordId);

            return Ok(result);
        }
    }
}
