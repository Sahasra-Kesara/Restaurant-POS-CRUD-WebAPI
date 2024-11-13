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
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _context.Foods.ToListAsync();
            return Ok(foods);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(Food food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllFoods), new { id = food.Id }, food);
        }

        // Update an existing food item
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFood(int id, Food updatedFood)
        {
            if (id != updatedFood.Id)
                return BadRequest("Food ID mismatch");

            var existingFood = await _context.Foods.FindAsync(id);
            if (existingFood == null)
                return NotFound("Food not found");

            // Update properties
            existingFood.Name = updatedFood.Name;
            existingFood.Price = updatedFood.Price;

            _context.Foods.Update(existingFood);
            await _context.SaveChangesAsync();

            return Ok(existingFood);
        }

        // Delete a food item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
                return NotFound("Food not found");

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Food item deleted successfully" });
        }
    }
}
