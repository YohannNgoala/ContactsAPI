using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsApi.Models;
using ContactsAPI.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace ContactsAPI.Controllers
{
    [Authorize]
    [Route("api/Contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly DatabaseContext _context;

        public ContactsController(ILogger<ContactsController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Retrieves all contacts from the list of contacts
        /// </summary>
        /// <remarks>To get the skills details, use GET request with Contacts/{id} </remarks>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ContactModel>>> GetContacts()
        {            
            return await _context.Contacts.ToListAsync();
        }

        /// <summary>
        /// Retrieves details of a contact from his id
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ContactModel>> GetContact(long id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }
            var skills = _context.Skills.Where(w => w.ContactModelId == id).ToList();
            foreach (var skill in skills)
            { 
                contact.SkillModel.Add(skill); 
            }
            return contact;
        }

        /// <summary>
        /// Insert a new contact in the list /!\ You need to be authenticated) /!\
        /// <remarks>
        /// /!\ Leave the Username and ContactModelId by default /!\
        /// </remarks>
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ContactModel>> PostContact(ContactModel contact)
        {
           if (contact.UserName != null || contact.ContactModelId != 0)
            {
                return Conflict("You must leave Username and ContactModelId by default ('string' and 0)");
            }
            contact.UserName = HttpContext.User.Identity.Name;
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContact), new { id = contact.ContactModelId }, contact);
        }

        /// <summary>
        /// Change a single contact in the list from ID /!\ You need to be authenticated) /!\
        /// </summary>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutContact(long id, ContactModel contact)
        {
            if (id != contact.ContactModelId)
            {
                return BadRequest("Contact Id missmatch");
            }
            var contactModel =  _context.Contacts.AsNoTracking().Where(w => w.ContactModelId == id).SingleOrDefault();
            if ( contactModel == null)
            {
                return NotFound("Contact doesn't exist");
            }
            if (contactModel.UserName != HttpContext.User.Identity.Name)
            {
                return Unauthorized("This is not your contact");
            }
            contact.UserName = HttpContext.User.Identity.Name;
            _context.Entry(contact).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!_context.Contacts.Any(c => c.ContactModelId == contact.ContactModelId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        /// <summary>
        /// Delete a contact from the list /!\ You need to be authenticated) /!\
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactModel>> DeleteContactModel(long id)
        {
            var contactModel = await _context.Contacts.FindAsync(id);
            if (contactModel == null)
            {
                return NotFound();
            }
            if (contactModel.UserName != HttpContext.User.Identity.Name)
            {
                return Unauthorized("This is not your contact");
            }
            _context.Contacts.Remove(contactModel);
            RemoveSkills(id);
            await _context.SaveChangesAsync();

            return contactModel;
        }

        private void RemoveSkills(long contactId)
        {
            var skills = _context.Skills.Where(s => s.ContactModelId == contactId);
            _context.Skills.RemoveRange(skills);
        }
    }
}
