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
    [Route("api/Api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly DatabaseContext _database;

        public ApiController(ILogger<ApiController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }


        [HttpGet]
        [Route("contacts")]
        public async Task<ActionResult<IEnumerable<ContactModel>>> GetContacts()
        {
            return await _database.Contacts.ToListAsync();
        }

        [HttpGet("{id}")]
        [Route("contacts")]
        public async Task<ActionResult<ContactModel>> GetContact(int id)
        {
            var contact = await _database.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpGet]
        [Route("skillNames")]
        public async Task<ActionResult<SkillNameModel>> GetSkillNames(long id)
        {
            var skillName = await _database.SkillNames.FindAsync(id);

            if (skillName == null)
            {
                return NotFound();
            }

            return skillName;
        }

        [HttpGet]
        [Route("skillLevels")]
        public async Task<ActionResult<SkillLevelModel>> GetSkillLevels(long id)
        {
            var skillLevel = await _database.SkillLevels.FindAsync(id);

            if (skillLevel == null)
            {
                return NotFound();
            }

            return skillLevel;
        }
        [HttpPost]
        [Route("contacts")]
        [AllowAnonymous]
        public async Task<ActionResult<ContactModel>> PostContact(ContactModel contact)
        {
            _database.Contacts.Add(contact);
            await _database.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        [HttpPost]
        [Route("skillNames")]
        [AllowAnonymous]
        public async Task<ActionResult<SkillNameModel>> PostSkill([FromBody] SkillNameModel skill)
        {
            if (_database.SkillNames.Any(s => s.Name == skill.Name))
            {
                return Conflict();
            }
            _database.SkillNames.Add(skill);
            await _database.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSkillNames), new { id = skill.Id }, skill);
        }

        [HttpPost]
        [Route("skillLevels")]
        [AllowAnonymous]
        public async Task<ActionResult<SkillLevelModel>> PostLevel([FromBody] SkillLevelModel level)
        {
            if (_database.SkillLevels.Any(s => s.Level == level.Level))
            {
                return Conflict();
            }
            _database.SkillLevels.Add(level);
            await _database.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSkillLevels), new { id = level.Id }, level);
        }

        [HttpPut]
        [Route("contacts")]
        [AllowAnonymous]
        public async Task<IActionResult> PutContact(long id, ContactModel contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }
            _database.Entry(contact).State = EntityState.Modified;
            try
            {
                await _database.SaveChangesAsync(); 
            }
            catch
            {
                if (!_database.Contacts.Any(c => c.Id == id))
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
    }
}
