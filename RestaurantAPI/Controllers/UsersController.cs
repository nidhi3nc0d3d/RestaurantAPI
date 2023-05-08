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
    public class UsersController : ControllerBase
    {
        private readonly RestaurantDBContext _context;



        public UsersController(RestaurantDBContext context)
        {
            _context = context;
        }





        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<bool>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'RestaurantDBContext.Users'  is null.");
            }
            if (UserExists(user.UserId) == true)
            {
                User matchedUser = _context.Users.Find(user.UserId);
                if (matchedUser.Password == user.Password)
                    return true;
                else
                    return false;
            }

            return false;
        }




        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}