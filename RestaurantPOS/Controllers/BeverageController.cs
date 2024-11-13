using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPOS.Data;
using RestaurantPOS.Models;
using System.Threading.Tasks;

namespace RestaurantPOS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeverageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BeverageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBeverages()
        {
            var beverages = await _context.Beverages.ToListAsync();
            return Ok(beverages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBeverage(Beverage beverage)
        {
            _context.Beverages.Add(beverage);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllBeverages), new { id = beverage.Id }, beverage);
        }

        // Update an existing beverage item
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBeverage(int id, Beverage updatedBeverage)
        {
            if (id != updatedBeverage.Id)
                return BadRequest("Beverage ID mismatch");

            var existingBeverage = await _context.Beverages.FindAsync(id);
            if (existingBeverage == null)
                return NotFound("Beverage not found");

            // Update properties
            existingBeverage.Name = updatedBeverage.Name;
            existingBeverage.Price = updatedBeverage.Price;

            _context.Beverages.Update(existingBeverage);
            await _context.SaveChangesAsync();

            return Ok(existingBeverage);
        }

        // Delete a beverage item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeverage(int id)
        {
            var beverage = await _context.Beverages.FindAsync(id);
            if (beverage == null)
                return NotFound("Beverage not found");

            _context.Beverages.Remove(beverage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Beverage item deleted successfully" });
        }
    }
}
