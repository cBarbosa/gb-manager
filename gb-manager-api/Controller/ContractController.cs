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
    public class ContractController : ControllerBase
    {
        private readonly IContractService service;

        public ContractController(IContractService _service)
        {
            service = _service;
        }

        [HttpPut("")]
        public async Task<IActionResult> Put([FromBody] CreateContractCommand cmd)
        {
            var result = await service.Create(cmd);

            return Ok(result);
        }

        [HttpPut("add-person")]
        public async Task<IActionResult> PutPerson(
            [FromBody] CreateContractCommand cmd)
        {
            var result = await service.AddPerson(cmd.RecordId.Value, cmd.PersonRecordId.Value);

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] CreateContractCommand cmd)
        {
            var result = await service.Update(cmd);

            return Ok(result);
        }

        [HttpGet("{recordId}")]
        public async Task<IActionResult> Post([FromRoute] Guid recordId)
        {
            var result = await service.GetByRecordId(recordId);

            return Ok(result);
        }
    }
}