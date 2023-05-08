using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;



namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly RestaurantDBContext _context;



        public DishesController(RestaurantDBContext context)
        {
            _context = context;
        }



        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }
            List<Dish> dishes = await _context.Dishes.ToListAsync();
            List<Dish> filteredDishes = dishes.FindAll(dish => dish.IsDeleted == false);
            return Ok(filteredDishes);
        }



        [HttpGet("catId={catId}")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes(int catId)
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }

            List<CategoryDish> categoryDishes = await _context.CategoryDishes.ToListAsync();
            List<int?> fileteredDishes = new List<int?>();
            categoryDishes.FindAll(cd => cd.IsDeleted == false && cd.CatId == catId).ForEach(cd => fileteredDishes.Add(cd.DishId));
            List<Dish> dishes = new List<Dish>();
            await _context.Dishes.ForEachAsync(dish =>
            {
                if (dish.IsDeleted == false && fileteredDishes.Contains(dish.DishId))
                    dishes.Add(dish);
            }
            );
            return Ok(dishes);
        }



        // GET: api/Dishes/5
        [HttpGet("dishId={id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }
            var dish = await _context.Dishes.FindAsync(id);



            if (dish == null || dish.IsDeleted)
            {
                return NotFound();
            }



            return dish;
        }



        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.DishId)
            {
                return BadRequest();
            }



            _context.Entry(dish).State = EntityState.Modified;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }



            return NoContent();
        }



        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{catId}")]
        public async Task<ActionResult<Dish>> PostDish(int catId, Dish dish)
        {
            if (_context.Dishes == null)
            {
                return Problem("Entity set 'RestaurantDBContext.Dishes'  is null.");
            }
            // adding new dish to dish table
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();



            // adding dish and its category to category_dish table
            CategoryDish categoryDish = new CategoryDish();
            categoryDish.CatId = catId;
            categoryDish.DishId = dish.DishId;
            _context.CategoryDishes.Add(categoryDish);
            await _context.SaveChangesAsync();



            return CreatedAtAction("GetDish", new { id = dish.DishId }, dish);
        }



        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null || dish.IsDeleted)
            {
                return NotFound();
            }
            List<CategoryDish> categoryDishes = await _context.CategoryDishes.ToListAsync();
            categoryDishes.ForEach(catDish =>
            {
                if (catDish.DishId == id && catDish.IsDeleted == false)
                {
                    catDish.IsDeleted = true;
                    _context.CategoryDishes.Update(catDish);
                    _context.SaveChanges();
                }
            });
            dish.IsDeleted = true;
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();



            return NoContent();
        }



        private bool DishExists(int id)
        {
            return (_context.Dishes?.Any(e => e.DishId == id)).GetValueOrDefault();
        }
    }
}