using ControleDeEstacionamento.Data;
using ControleDeEstacionamento.Models;
using ControleDeEstacionamento.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var vehicles = await context.Vehicles.ToListAsync();
                if (vehicles.Count == 0)
                    return NotFound("Não temos veiculos registrados");

                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVehicleByIdAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var vehicles = await context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
                if (vehicles == null)
                    return NotFound("Veiculo não encontrado");
                return Ok(vehicles);
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha ao buscar veiculos");
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> PostVehicleAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] VehicleModel model)
        {
            try
            {
                var company = context.Companies.FirstOrDefault(x => x.Id == model.ParkingId);
                if (company == null)
                    return NotFound("Nenhum estacionamento encontrado com o Id informado");

                var plate = context.Vehicles.FirstOrDefault(x => x.LicensePlate == model.LicensePlate);
                if (plate != null)
                    return BadRequest("Já existe um veiculo registrado com essa placa");

                var newVehicle = new Vehicle
                {
                    LicensePlate = model.LicensePlate,
                    Year = model.Year,
                    Model = model.Model,
                    Color = model.Color,
                    EntryTime = DateTime.Now,
                    ParkingId = model.ParkingId


                };
                await context.Vehicles.AddAsync(newVehicle);
                await context.SaveChangesAsync();

                return Created($"{newVehicle.Id}", newVehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutVehicleAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] EditVehicleModel model,
            [FromRoute] int id)
        {
            try
            {
                var vehicle = await context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
                if (vehicle == null)
                    return NotFound("Veiculo não encontrado");

                var plate = context.Vehicles.FirstOrDefault(x => x.LicensePlate == model.LicensePlate);
                if (plate != null)
                    return BadRequest("Já existe um veiculo registrado com essa placa");

                vehicle.LicensePlate = model.LicensePlate;
                vehicle.Year = model.Year;
                vehicle.Model = model.Model;
                vehicle.Color = model.Color;
                

                context.Vehicles.Update(vehicle);
                await context.SaveChangesAsync();

                return Ok(vehicle);

            }
            catch (Exception)
            {
                return StatusCode(500, "Não foi possível atualizar os dados do veiculo");
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicleAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
               var vehicle = await context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
                if (vehicle == null) 
                    return NotFound("Veiculo não encontrado");

                vehicle.ExitTime = DateTime.Now;
                context.Vehicles.Update(vehicle);
                
                context.Vehicles.Remove(vehicle);
                await context.SaveChangesAsync();

                return Ok(vehicle);
            }
            catch (Exception)
            {
                return StatusCode(500, "Não foi possível deletar o veiculo");
            }

        }
    }
}