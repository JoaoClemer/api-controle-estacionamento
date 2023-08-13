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
    public class CompanyController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCompaniesAsync([FromServices] ParkingDbContext context)
        {
            try
            {
                var companies = await context.Companies.AsNoTracking().ToListAsync();
                if(companies.Count == 0)
                    return NotFound(new ResultModel<string>("We have no registered companies!"));

                return Ok(new ResultModel<List<Company>>(companies));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
            
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCompanyById(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var company = await context.Companies.AsNoTracking().Include(x=> x.Users).Include(x=> x.Parkings).FirstOrDefaultAsync(x => x.Id == id);
                if (company == null)
                    return NotFound(new ResultModel<string>("This company does not exist!"));

                return Ok(new ResultModel<Company>(company));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCompanyAsync(
            [FromServices]ParkingDbContext context,
            [FromBody] CompanyModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<Company>(ModelState.GetErrors()));
            try
            {
                var newCompany = new Company
                {
                    Name = model.Name,
                    Country = model.Country,
                };
                await context.Companies.AddAsync(newCompany);
                await context.SaveChangesAsync();

                return Created($"{newCompany.Id}", new ResultModel<Company>(newCompany));
            }
            catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCompanyAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] CompanyModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<Company>(ModelState.GetErrors()));
            try
            {
                var companies = await context.Companies.FirstOrDefaultAsync(x => x.Id == id);
                if (companies == null)
                    return BadRequest(new ResultModel<string>("This company does not exist!"));

                companies.Name = model.Name;
                companies.Country = model.Country;
               
                context.Companies.Update(companies);
                await context.SaveChangesAsync();

                return Ok(new ResultModel<Company>(companies));
            }catch
            {
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCompanyAsync(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id) 
        {
            try
            {
                var company = await context.Companies.FirstOrDefaultAsync(x => x.Id == id);
                if (company == null)
                    return BadRequest(new ResultModel<string>("This company does not exist!"));

                context.Companies.Remove(company);
                await context.SaveChangesAsync();
                return Ok(new ResultModel<Company>(company));
            }
            catch{ 
                return StatusCode(500, new ResultModel<string>("Internal server failure!"));
            }
        }

    }
}
