using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactsApi.Models;
using System.Data.OleDb;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public SkillsController(DatabaseContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieves all skills from the list of skills
        /// </summary>
        // GET: api/Skills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillModel>>> GetSkill()
        {
            return await _context.Skills.ToListAsync();
        }
        /// <summary>
        /// Retrieves details of a skill from its id
        /// </summary>
        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillModel>> GetSkill(long id)
        {
            var skill = await _context.Skills.FindAsync(id);

            if (skill == null)
            {
                return NotFound();
            }

            return skill;
        }


        /// <summary>
        /// Change a single skill in the list from its ID
        /// </summary>
        // PUT: api/Skills/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill(long id, SkillModel skill)
        {
            if (id != skill.Id)
            {
                return BadRequest();
            }
            
            var contactExist = _context.Contacts.Any(c => c.ContactModelId == skill.ContactModelId);
            if (!contactExist || CheckContactChanged(skill))
                return NotFound("Contact Id is incorrect");
            if (CheckNameChanged(skill))
            {
                if (CheckNameExist(skill))
                {
                    return Conflict("This skill name is already used");
                }
            }
                        
            _context.Entry(skill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(skill);
        }


        /// <summary>
        /// Insert a new skill in the list
        /// </summary>
        /// <remarks>The skill's ContactModelId should match with an existing contact's ContactModelId.
        /// The a contact can't have two skills with the same name</remarks>
        // POST: api/Skills
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SkillModel>> PostSkill(SkillModel skill)
        {
            var contactExist = _context.Contacts.Any(c => c.ContactModelId == skill.ContactModelId);
            if (!contactExist)
                return NotFound("Contact Id is incorrect");
            if (_context.Skills.Any(s => s.ContactModelId == skill.ContactModelId && s.Name == skill.Name))
                return Conflict("Ths skill already exist, do a PUT request to change it.");
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSkill", new { id = skill.Id }, skill);
        }


        /// <summary>
        /// Delete a skill from the list
        /// </summary>
        // DELETE: api/Skills/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SkillModel>> DeleteSkill(long id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return skill;
        }

        private bool CheckNameExist(SkillModel skill)
        {
            return _context.Skills.Any(a => a.Name == skill.Name);
        }

        private bool CheckNameChanged(SkillModel skill)
        {
            var oldSkill = _context.Skills.AsNoTracking().First(f => f.Id == skill.Id);
            return (skill.Name != oldSkill.Name);
        }
        private bool CheckContactChanged(SkillModel skill)
        {
            var oldSkill = _context.Skills.AsNoTracking().First(f => f.Id == skill.Id);
            return (skill.ContactModelId != oldSkill.ContactModelId);
        }
        private bool SkillExists(long id)
        {
            return _context.Skills.Any(e => e.Id == id);
        }
    }
}
