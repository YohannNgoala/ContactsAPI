using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }
       
        /// <summary>
        /// Add a new User
        /// </summary>
        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserModel>> Register(UserModel user)
        {
            var userExist = _context.Users.Any(u => u.UserName == user.UserName);
            if (userExist)
                return Conflict("This username is already used.");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("Unregister/{username}")]       
        public async Task<ActionResult<UserModel>> Unregister(string userName)
        {
            if (userName != HttpContext.User.Identity.Name)
            {
                return Unauthorized("You can't delete an other user");
            }
            var user = await _context.Users.FindAsync(userName);
            
            _context.Users.Remove(user);
            var contacts = _context.Contacts.Where(c => c.UserName == userName);
            foreach(var contact in contacts)
            {
                var skills = _context.Skills.Where(s => s.ContactModelId == contact.ContactModelId);
                _context.Skills.RemoveRange(skills);
            }
            _context.Contacts.RemoveRange(contacts);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
