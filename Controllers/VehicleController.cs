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
    public class VehicleController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetVehiclesAsync(
            [FromServices] ParkingDbContext context)
        {
            try
            {
                var vehicles = await context.Vehicles.AsNoTracking().ToListAsync();
                if (vehicles.Count == 0)
                    return NotFound(new ResultModel<string>("We have no registered vehicles!"));

                return Ok(new ResultModel<List<Vehicle>>(vehicles));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVehicleByIdAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var vehicles = await context.Vehicles.AsNoTracking().Include(x=> x.Parking).FirstOrDefaultAsync(x => x.Id == id);
                if (vehicles == null)
                    return NotFound(new ResultModel<string>("This vehicle does not exist!"));
                
                return Ok(new ResultModel<Vehicle>(vehicles));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> PostVehicleAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] VehicleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<Vehicle>(ModelState.GetErrors()));
            try
            {

                var parking = context.Parkings.FirstOrDefault(x => x.Id == model.ParkingId);
                if (parking == null)
                    return NotFound(new ResultModel<string>("No parking found with the given id"));
                
                //if (parking.Vehicles.Count() >= parking.TotalParkingSpots)
                //    return BadRequest("O estacionamento está lotado");

                var plate = context.Vehicles.FirstOrDefault(x => x.LicensePlate == model.LicensePlate);
                if (plate != null)
                    return BadRequest(new ResultModel<string>("There is already a vehicle registered with this license plate."));

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

                return Created($"{newVehicle.Id}",new ResultModel<Vehicle>(newVehicle));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }

        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutVehicleAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] EditVehicleModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<Vehicle>(ModelState.GetErrors()));
            try
            {
                var vehicle = await context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
                if (vehicle == null)
                    return NotFound(new ResultModel<string>("This vehicle does not exist!"));

                var plate = context.Vehicles.FirstOrDefault(x => x.LicensePlate == model.LicensePlate);
                if (plate != null)
                    return BadRequest(new ResultModel<string>("There is already a vehicle registered with this license plate."));

                vehicle.LicensePlate = model.LicensePlate;
                vehicle.Year = model.Year;
                vehicle.Model = model.Model;
                vehicle.Color = model.Color;


                context.Vehicles.Update(vehicle);
                await context.SaveChangesAsync();

                return Ok(new ResultModel<Vehicle>(vehicle));

            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
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
                    return NotFound(new ResultModel<string>("This vehicle does not exist!"));

                vehicle.ExitTime = DateTime.Now;
                context.Vehicles.Update(vehicle);

                context.Vehicles.Remove(vehicle);
                await context.SaveChangesAsync();

                return Ok(new ResultModel<Vehicle>(vehicle));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }

        }
     
    }
}