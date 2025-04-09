using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models;
using WebStore.Validators;

namespace WebStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        public CustomersController(ECommerceDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Customers.AsNoTracking().ToListAsync());

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            var validator = new CustomerValidator();
            var result = validator.Validate(customer);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound("Customer not found");
            return Ok(customer);
        }
    }

}
