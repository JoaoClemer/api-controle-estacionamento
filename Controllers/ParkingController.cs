using ControleDeEstacionamento.Data;
using ControleDeEstacionamento.Extensions;
using ControleDeEstacionamento.Models;
using ControleDeEstacionamento.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstacionamento.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ParkingController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetParkingsAsync(
            [FromServices] ParkingDbContext context)
        {
            try
            {
                var parkings = await context.Parkings.AsNoTracking().ToListAsync();
                if (parkings.Count == 0)
                    return NotFound(new ResultModel<string>("We have no registered parkings!"));

                return Ok(new ResultModel<List<Parking>>(parkings));

            }catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!")
);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetParkingById(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var parking = await context.Parkings.AsNoTracking().Include(x => x.Company).Include(x => x.Vehicles).FirstOrDefaultAsync(x => x.Id == id);
                if (parking == null)
                    return NotFound(new ResultModel<string>("This parking does not exist!"));

                return Ok(new ResultModel<Parking>(parking));
            }

            catch
            {
                    return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostParkingAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] ParkingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<Parking>(ModelState.GetErrors()));
            try
            {
                var company = context.Companies.FirstOrDefault(x => x.Id == model.CompanyId);
                if (company == null)
                    return NotFound(new ResultModel<string>("No company found with the given id"));

                var newParking = new Parking
                {
                    TotalParkingSpots = model.TotalParkingSpots,
                    CompanyId = company.Id
                };
                await context.Parkings.AddAsync(newParking);
                await context.SaveChangesAsync();

                return Created($"/{newParking.Id}", new ResultModel<Parking>(newParking));

            }
            catch 
            { 
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutParkingAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] EditParkingModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<Parking>(ModelState.GetErrors()));
            try
            {

                var parkings = await context.Parkings.FirstOrDefaultAsync(x => x.Id == id);
                if (parkings == null)
                    return NotFound(new ResultModel<string>("This parking does not exist!"));

                parkings.TotalParkingSpots = model.TotalParkingSpots;

                context.Parkings.Update(parkings);
                await context.SaveChangesAsync();

                return Ok(new ResultModel<Parking>(parkings));

            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteParkingAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var parking = await context.Parkings.FirstOrDefaultAsync(x => x.Id == id);
                if (parking == null)
                    return BadRequest(new ResultModel<string>("This parking does not exist!"));

                context.Parkings.Remove(parking);
                await context.SaveChangesAsync();
                return Ok(new ResultModel<Parking>(parking));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

    }
}
