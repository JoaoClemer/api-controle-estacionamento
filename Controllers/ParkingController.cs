using ControleDeEstacionamento.Data;
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
                var parkings = await context.Parkings.AsNoTracking().Include(x=> x.Company).Include(x=> x.Vehicles).ToListAsync();
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
                var parking = await context.Parkings.AsNoTracking().Include(x => x.Company).Include(x => x.Vehicles).FirstOrDefaultAsync(x => x.Id == id);
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
            [FromBody] ParkingModel model)
        {
            try
            {
                var company = context.Companies.FirstOrDefault(x => x.Id == model.CompanyId);
                if (company == null)
                    return NotFound("Nenhuma empresa encontrada com o Id informado");

                var newParking = new Parking
                {
                    TotalParkingSpots = model.TotalParkingSpots,
                    CompanyId = company.Id
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
            [FromBody] ParkingModel model,
            [FromRoute] int id)
        {
            try
            {
                var parkings = await context.Parkings.FirstOrDefaultAsync(x => x.Id == id);
                if (parkings == null)
                    return NotFound("Parking não encontrado");

                parkings.TotalParkingSpots = model.TotalParkingSpots;

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
