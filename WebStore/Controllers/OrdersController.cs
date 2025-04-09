using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.DTOs;
using WebStore.Models;
using WebStore.Validators;

namespace WebStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        public OrdersController(ECommerceDbContext context) => _context = context;

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestDto request)
        {
            var validator = new OrderRequestValidator();
            var result = validator.Validate(request);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            var customer = await _context.Customers.FindAsync(request.CustomerId);
            if (customer == null) return NotFound("Customer not found");

            var products = await _context.Products.Where(p => request.ProductIds.Contains(p.Id)).ToListAsync();
            if (!products.Any()) return BadRequest("Invalid product selection.");

            var outOfStock = products.Where(p => p.Stock <= 0).Select(p => p.Name).ToList();
            if (outOfStock.Any())
                return BadRequest($"The following product(s) are out of stock: {string.Join(", ", outOfStock)}");

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending.ToString(),
                TotalPrice = products.Sum(p => p.Price),
                OrderProducts = products.Select(p => new OrderProduct { ProductId = p.Id }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, new OrderDto
            {
                OrderId = order.Id,
                CustomerName = customer.Name,
                OrderStatus = order.Status,
                NumberOfProducts = order.OrderProducts.Count
            });
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders.Include(o => o.Customer)
                                             .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                                             .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound("Order not found");

            return Ok( new OrderDto
            {
                OrderId = order.Id,
                CustomerName = order.Customer.Name,
                OrderStatus = order.Status,
                NumberOfProducts = order.OrderProducts.Count
            });
        }

        [HttpPost("updateOrderStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                                             .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound("Order not found");

            order.Status = OrderStatus.Delivered.ToString();
            foreach (var op in order.OrderProducts)
            {
                op.Product.Stock -= 1;
            }
            await _context.SaveChangesAsync();
            return Ok("Order status updated and product stock decreased.");
        }
    }
}
