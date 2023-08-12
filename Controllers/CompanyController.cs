using ControleDeEstacionamento.Data;
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
        [HttpGet("")]
        public async Task<IActionResult> GetCompaniesAsync([FromServices] ParkingDbContext context)
        {
            try
            {
                var companies = await context.Companies.ToListAsync();
                if(companies.Count == 0)
                    return NotFound("Não temos empresas cadastradas");

                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCompanyById(
            [FromServices] ParkingDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var company = await context.Companies.FirstOrDefaultAsync(x => x.Id == id);
                if (company == null)
                    return NotFound("Não foi encontrado essa empresa");

                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostCompanyAsync(
            [FromServices]ParkingDbContext context,
            [FromBody] CompanyModel model)
        {
            try
            {
                var newCompany = new Company
                {
                    Name = model.Name,
                    Country = model.Country,
                };
                await context.Companies.AddAsync(newCompany);
                await context.SaveChangesAsync();

                return Created($"v1/users/{newCompany.Id}", newCompany);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCompanyAsync(
            [FromServices] ParkingDbContext context,
            [FromBody] Company company,
            [FromRoute] int id)
        {
            try
            {
                var companies = await context.Companies.FirstOrDefaultAsync(x => x.Id == id);
                if (companies == null)
                    return BadRequest("Empresa não encontrada");

                companies.Name = company.Name;
                companies.Country = company.Country;
               
                context.Companies.Update(companies);
                await context.SaveChangesAsync();

                return Ok(companies);
            }catch (Exception ex)
            {
                return StatusCode(500, ex);
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
                    return BadRequest("Empresa não encontrada");

                context.Companies.Remove(company);
                await context.SaveChangesAsync();
                return Ok(company);
            }
            catch (Exception ex) { 
                return StatusCode(500, ex);
            }
        }

    }
}
