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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService service;

        public PaymentController(IPaymentService _service)
        {
            service = _service;
        }

        [HttpGet("{recordId}/installment")]
        public async Task<IActionResult> GetInstallments(
            [FromRoute] string recordId)
        {
            //var result = await service.GetContracts(recordId);
            //return Ok(result);
            var result = await service.GetTest(recordId);
            return Ok(result);
        }
    }
}