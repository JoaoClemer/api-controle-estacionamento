using ControleDeEstacionamento.Data;
using ControleDeEstacionamento.Extensions;
using ControleDeEstacionamento.ModelView;
using ControleDeEstacionamento.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstacionamento.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class AccountController : ControllerBase
    {
      
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginModel model,
            [FromServices] ParkingDbContext context,
            [FromServices] TokenService tokenService)
        {
           if(!ModelState.IsValid)
                return BadRequest(new ResultModel<string>(ModelState.GetErrors()));

            try
            {
                var user = await context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == model.Username && x.PasswordHash == model.PasswordHash);
                if (user == null)
                    return BadRequest(new ResultModel<string>("Invalid username or password!"));

                var token = tokenService.GenerateToken(user);
                return Ok(new ResultModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

       
    }
}
