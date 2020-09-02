using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsApi.Models;

namespace ContactsAPI.Controllers
{
    [Route("api/ContactsItems")]
    [ApiController]
    public class ContactsItemsController : ControllerBase
    {
        private readonly ContactsContext _context;

        public ContactsItemsController(ContactsContext context)
        {
            _context = context;
        }

        // GET: api/ContactsItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactsItem>>> GetContactsItems()
        {
            return await _context.ContactsItems.ToListAsync();
        }

        // GET: api/ContactsItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactsItem>> GetContactsItem(long id)
        {
            var contactsItem = await _context.ContactsItems.FindAsync(id);

            if (contactsItem == null)
            {
                return NotFound();
            }

            return contactsItem;
        }

        // PUT: api/ContactsItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactsItem(long id, ContactsItem contactsItem)
        {
            if (id != contactsItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactsItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsItemExists(id))
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

        // POST: api/ContactsItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContactsItem>> PostContactsItem(ContactsItem contactsItem)
        {
            _context.ContactsItems.Add(contactsItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContactsItem), new { id = contactsItem.Id }, contactsItem);
        }

        // DELETE: api/ContactsItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactsItem>> DeleteContactsItem(long id)
        {
            var contactsItem = await _context.ContactsItems.FindAsync(id);
            if (contactsItem == null)
            {
                return NotFound();
            }

            _context.ContactsItems.Remove(contactsItem);
            await _context.SaveChangesAsync();

            return contactsItem;
        }

        private bool ContactsItemExists(long id)
        {
            return _context.ContactsItems.Any(e => e.Id == id);
        }
    }
}
