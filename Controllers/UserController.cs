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
    public class UserController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetUsersAsync(
            [FromServices] ParkingDbContext context) 
        {
            try
            {
                
                var users = await context.Users.AsNoTracking().ToListAsync();

                if(users.Count == 0)
                    return NotFound(new ResultModel<string>("We have no registered users!"));  

                return Ok(new ResultModel<List<User>>(users));
            }catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserByIdAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute]int id)
        {
            try
            {
                var user = await context.Users.AsNoTracking().Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
                if(user == null)
                    return NotFound(new ResultModel<string>("This user does not exist!"));
                
                return Ok(new ResultModel<User>(user));
            }
            catch(Exception)
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostUserAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<User>(ModelState.GetErrors()));
            try
            {
                var company = context.Companies.FirstOrDefault(x => x.Id == model.CompanyId);
                if (company == null)
                    return NotFound(new ResultModel<string>("No company found with the given id"));

                var newUser = new User
                {
                    Name = model.Name,
                    Username = model.Username,
                    PasswordHash = model.PasswordHash,
                    Role = UserRole.VehicleRegister,
                    CompanyId = model.CompanyId,
                    IsActive = true

                };
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                return Created($"{newUser.Id}", new ResultModel<User>(newUser));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutUserAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] EditUserModel user,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<User>(ModelState.GetErrors()));
            try
            {
                var users = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (users == null)
                    return NotFound(new ResultModel<string>("This user does not exist!"));

                users.Name = user.Name;
                users.Username = user.Username;
                users.PasswordHash = user.PasswordHash;
               
            
                context.Users.Update(users);
                await context.SaveChangesAsync();

                return Ok(new ResultModel<User>(users));
            
            }catch (Exception) {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id
            )
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                    return NotFound(new ResultModel<string>("This user does not exist!"));
                
                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return Ok(new ResultModel<User>(user));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

    }

    
}
