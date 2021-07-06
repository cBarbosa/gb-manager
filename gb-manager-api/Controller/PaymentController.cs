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
    public class PaymentController : ControllerBase
    {
        [HttpGet("{recordId}/installment")]
        public async Task<IActionResult> GetInstallments(
            [FromRoute] Guid recordId)
        {
            //var result = await service.GetContracts(recordId);

            //return Ok(result);
            return Ok("ok");
        }
    }
}
