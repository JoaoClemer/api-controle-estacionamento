using ControleDeEstacionamento.Data;
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
                
                var users = await context.Users.AsNoTracking().Include(x => x.Company).ToListAsync();

                if(users.Count == 0)
                    return NotFound("Não temos usuários registrados");  

                return Ok(users);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserByIdAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute]int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if(user == null)
                    return NotFound("Usuário não encontrado");
                return Ok(user);
            }
            catch(Exception)
            {
                return StatusCode(500, "Falha ao buscar usuários");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostUserAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] UserModel user)
        {
            try
            {
                var company = context.Companies.FirstOrDefault(x => x.Id == user.CompanyId);
                if (company == null)
                    return NotFound("Nenhuma empresa encontrada com o Id informado");

                var newUser = new User
                {
                    Name = user.Name,
                    Username = user.Username,
                    PasswordHash = user.PasswordHash,
                    Role = UserRole.VehicleRegister,
                    CompanyId = user.CompanyId,
                    IsActive = true

                };
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                return Created($"{newUser.Id}", newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutUserAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] User user,
            [FromRoute] int id)
        {
            try
            {
                var users = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (users == null)
                    return NotFound("Usuário não encontrado");

                users.Name = user.Name;
                users.Username = user.Username;
                users.Role = user.Role;
                users.IsActive = user.IsActive;
            
                context.Users.Update(users);
                await context.SaveChangesAsync();

                return Ok(users);
            
            }catch (Exception) {
                return StatusCode(500, "Não foi possível atualizar os dados do usuário");
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
                    return NotFound("Usuário não encontrado");
                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Não foi possível deletar o usuário");
            }
        }
    }
}
