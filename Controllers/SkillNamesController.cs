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
    [Route("api/SkillNames")]
    [ApiController]
    public class SkillNamesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public SkillNamesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/SkillNameModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillNameModel>>> GetSkillNames()
        {
            return await _context.SkillNames.ToListAsync();
        }

        // GET: api/SkillNameModels/5
        [HttpGet("{name}")]
        public async Task<ActionResult<SkillNameModel>> GetSkillName(String name)
        {
            var skillName = await _context.SkillNames.FindAsync(name);

            if (skillName == null)
            {
                return NotFound();
            }

            return skillName;
        }

        // PUT: api/SkillNameModels/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{name}")]
        public async Task<IActionResult> PutSkillName(String name, SkillNameModel skillName)
        {
            if (name != skillName.Name)
            {
                return BadRequest();
            }
            if (_context.SkillNames.Any(s => s.Name == skillName.Name))
            {
                return Conflict();
            }

            _context.Entry(skillName).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillNameExists(name))
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

        // POST: api/SkillNameModels
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SkillNameModel>> PostSkillName(SkillNameModel skillName)
        {
            if (_context.SkillNames.Any(s => s.Name == skillName.Name))
            {
                return Conflict();
            }
            _context.SkillNames.Add(skillName);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSkillName", new { name = skillName.Name }, skillName);
        }

        // DELETE: api/SkillNameModels/5
        [HttpDelete("{name}")]
        public async Task<ActionResult<SkillNameModel>> DeleteSkillName(String name)
        {
            var skillName = await _context.SkillNames.FindAsync(name);
            if (skillName == null)
            {
                return NotFound();
            }

            _context.SkillNames.Remove(skillName);
            await _context.SaveChangesAsync();

            return skillName;
        }

        private bool SkillNameExists(String name)
        {
            return _context.SkillNames.Any(e => e.Name == name);
        }
    }
}
