using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studySpaceWebApp.Models;

namespace studySpaceWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollOptionsApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;

        public PollOptionsApiController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: api/PollOptionsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollOption>>> GetPollOptions()
        {
            return await _context.PollOptions.ToListAsync();
        }

        // GET: api/PollOptionsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollOption>> GetPollOption(int id)
        {
            var pollOption = await _context.PollOptions.FindAsync(id);

            if (pollOption == null)
            {
                return NotFound();
            }

            return pollOption;
        }

        // PUT: api/PollOptionsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollOption(int id, PollOption pollOption)
        {
            if (id != pollOption.OptionId)
            {
                return BadRequest();
            }

            _context.Entry(pollOption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollOptionExists(id))
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

        // POST: api/PollOptionsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PollOption>> PostPollOption(PollOption pollOption)
        {
            _context.PollOptions.Add(pollOption);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollOption", new { id = pollOption.OptionId }, pollOption);
        }

        // DELETE: api/PollOptionsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePollOption(int id)
        {
            var pollOption = await _context.PollOptions.FindAsync(id);
            if (pollOption == null)
            {
                return NotFound();
            }

            _context.PollOptions.Remove(pollOption);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PollOptionExists(int id)
        {
            return _context.PollOptions.Any(e => e.OptionId == id);
        }
    }
}
