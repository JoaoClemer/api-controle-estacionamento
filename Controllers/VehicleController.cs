using ControleDeEstacionamento.Data;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeEstacionamento.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class VehicleController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetVehiclesAsync(
            [FromServices] ParkingDbContext context)
        {
            return Ok();

        }
    }
}
