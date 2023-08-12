using ControleDeEstacionamento.Data;
using ControleDeEstacionamento.Models;
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
                var parkings = await context.Parkings.ToListAsync();
                if (parkings.Count == 0)
                    return NotFound("Não temos parkings registrados");

                return Ok(parkings);

            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetParkingById(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var parking = await context.Parkings.FirstOrDefaultAsync(x => x.Id == id);
                if (parking == null)
                    return NotFound("Parking não encontrado");

                return Ok(parking);
            }catch(Exception ex) 
            {
                    return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostParkingAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] Parking parking)
        {
            try
            {
                var newParking = new Parking
                {
                    Id = 0,
                    TotalParkingSpots = parking.TotalParkingSpots
                };
                await context.Parkings.AddAsync(newParking);
                await context.SaveChangesAsync();

                return Created($"/{newParking.Id}", newParking);

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutParkingAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] Parking parking,
            [FromRoute] int id)
        {
            try
            {
                var parkings = await context.Parkings.FirstOrDefaultAsync(x => x.Id == id);
                if (parkings == null)
                    return NotFound("Parking não encontrado");

                parkings.TotalParkingSpots = parking.TotalParkingSpots;

                context.Parkings.Update(parkings);
                await context.SaveChangesAsync();
                return Ok(parkings);

            }
            catch(Exception ex) 
            {
                return StatusCode(500, ex);
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
                    return BadRequest("Empresa não encontrada");

                context.Parkings.Remove(parking);
                await context.SaveChangesAsync();
                return Ok(parking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
