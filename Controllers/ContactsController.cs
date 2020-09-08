using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace ContactsAPI.Controllers
{
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
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactModel>>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }


        [HttpGet("{id}")]
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
        /// Insert a new contact in the list
        /// </summary>
        /// <param name=<em>"contact"</em>>New contact to be inserted</param>
        [HttpPost]
        public async Task<ActionResult<ContactModel>> PostContact(ContactModel contact)
        {
           
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContact), new { id = contact.ContactModelId }, contact);
        }

        /// <summary>
        /// Change a single contact in the list
        /// </summary>
        /// <param name=<em>"id"</em>>The id of the contact to be changed</param>
        /// <param name=<em>"value"</em>>The new values</param>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutContact(long id, ContactModel contact)
        {
            if (id != contact.ContactModelId)
            {
                return BadRequest();
            }
         
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
            return NoContent();
        }

        /// <summary>
        /// Delete an contact from the list
        /// </summary>
        /// <param name=<em>"id"</em>>id of the contact to be deleted</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactModel>> DeleteContactModel(long id)
        {
            var contactModel = await _context.Contacts.FindAsync(id);
            if (contactModel == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contactModel);
            await _context.SaveChangesAsync();

            return contactModel;
        }
    }
}
