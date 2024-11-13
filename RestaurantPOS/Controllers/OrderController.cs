using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPOS.Data;
using RestaurantPOS.Models;

namespace RestaurantPOS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItem> orderItems)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                OrderItems = orderItems
            };

            // Calculate total amount
            order.TotalAmount = orderItems.Sum(item => item.Price * item.Quantity);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpGet("{orderId}/GenerateBill")]
        public async Task<IActionResult> GenerateBill(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            return Ok(new
            {
                OrderId = order.Id,
                Date = order.OrderDate,
                Items = order.OrderItems.Select(item => new
                {
                    item.ItemName,
                    item.ItemType,
                    item.Quantity,
                    TotalPrice = item.Price * item.Quantity
                }),
                TotalAmount = order.TotalAmount
            });
        }

        // Update (Edit) an Order
        [HttpPut("EditOrder/{orderId}")]
        public async Task<IActionResult> EditOrder(int orderId, [FromBody] List<OrderItem> updatedOrderItems)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            // Update the order items and recalculate the total amount
            order.OrderItems.Clear();
            foreach (var item in updatedOrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ItemType = item.ItemType,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            order.TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity);

            await _context.SaveChangesAsync();
            return Ok(order);
        }

        // Delete an Order
        [HttpDelete("DeleteOrder/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order deleted successfully." });
        }
    }
}
