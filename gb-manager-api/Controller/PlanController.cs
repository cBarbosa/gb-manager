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
    public class PlanController : ControllerBase
    {
        private readonly IPlanService service;

        public PlanController(IPlanService _service)
        {
            service = _service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var result = await service.GetActives();

            return Ok(result);
        }
    }
}