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
    public class MenusController : ControllerBase
    {
        private readonly RestaurantDBContext _context;



        public MenusController(RestaurantDBContext context)
        {
            _context = context;
        }



        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus()
        {
            if (_context.Menus == null)
            {
                return NotFound();
            }
            List<Menu> menus = await _context.Menus.ToListAsync();
            List<Menu> filteredMenu = menus.FindAll(menu => menu.IsDeleted == false);
            return Ok(filteredMenu);
        }



        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu(int id)
        {
            if (_context.Menus == null)
            {
                return NotFound();
            }
            var menu = await _context.Menus.FindAsync(id);



            if (menu == null || menu.IsDeleted)
            {
                return NotFound();
            }



            return menu;
        }



        /*// PUT: api/Menus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, Menu menu)
        {
            if (id != menu.MenuId)
            {
                return BadRequest();
            }

 

            _context.Entry(menu).State = EntityState.Modified;

 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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
*/
        // POST: api/Menus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'RestaurantDBContext.Menus'  is null.");
            }
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();



            return CreatedAtAction("GetMenu", new { id = menu.MenuId }, menu);
        }



        /* // DELETE: api/Menus/5
         [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteMenu(int id)
         {
             if (_context.Menus == null)
             {
                 return NotFound();
             }
             var menu = await _context.Menus.FindAsync(id);
             if (menu == null)
             {
                 return NotFound();
             }

  

             _context.Menus.Remove(menu);
             await _context.SaveChangesAsync();

  

             return NoContent();
         }
 */
        private bool MenuExists(int id)
        {
            return (_context.Menus?.Any(e => e.MenuId == id)).GetValueOrDefault();
        }
    }
}