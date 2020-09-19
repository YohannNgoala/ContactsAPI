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
    }
}
