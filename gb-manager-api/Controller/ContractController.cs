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
    }
}