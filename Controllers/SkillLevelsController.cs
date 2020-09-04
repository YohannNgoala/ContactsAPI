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
    [Route("api/SkillLevels")]
    [ApiController]
    public class SkillLevelsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public SkillLevelsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/SkillLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillLevelModel>>> GetSkillLevels()
        {
            return await _context.SkillLevels.ToListAsync();
        }

        // GET: api/SkillLevels/5
        [HttpGet("{level}")]
        public async Task<ActionResult<SkillLevelModel>> GetSkillLevel(String level)
        {
            var skillLevel = await _context.SkillLevels.FindAsync(level);

            if (skillLevel == null)
            {
                return NotFound();
            }

            return skillLevel;
        }

        // PUT: api/SkillLevels/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{level}")]
        public async Task<IActionResult> PutSkillLevel(String level, SkillLevelModel skillLevel)
        {
            if (level != skillLevel.Level)
            {
                return BadRequest();
            }

            _context.Entry(skillLevel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillLevelExists(level))
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

        // POST: api/SkillLevels
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SkillLevelModel>> PostSkillLevel(SkillLevelModel skillLevel)
        {
            if (_context.SkillLevels.Any(s => s.Level == skillLevel.Level))
            {
                return Conflict();
            }
            _context.SkillLevels.Add(skillLevel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSkillLevel", new { level = skillLevel.Level }, skillLevel);
        }

        // DELETE: api/SkillLevels/5
        [HttpDelete("{level}")]
        public async Task<ActionResult<SkillLevelModel>> DeleteSkillLevel(String level)
        {
            var skillLevel = await _context.SkillLevels.FindAsync(level);
            if (skillLevel == null)
            {
                return NotFound();
            }

            _context.SkillLevels.Remove(skillLevel);
            await _context.SaveChangesAsync();

            return skillLevel;
        }

        private bool SkillLevelExists(String level)
        {
            return _context.SkillLevels.Any(e => e.Level == level);
        }
    }
}
