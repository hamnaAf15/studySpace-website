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
    public class PollVotesApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;

        public PollVotesApiController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: api/PollVotesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollVote>>> GetPollVotes()
        {
            return await _context.PollVotes.ToListAsync();
        }

        // GET: api/PollVotesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollVote>> GetPollVote(int id)
        {
            var pollVote = await _context.PollVotes.FindAsync(id);

            if (pollVote == null)
            {
                return NotFound();
            }

            return pollVote;
        }

        // PUT: api/PollVotesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollVote(int id, PollVote pollVote)
        {
            if (id != pollVote.VoteId)
            {
                return BadRequest();
            }

            _context.Entry(pollVote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollVoteExists(id))
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

        // POST: api/PollVotesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PollVote>> PostPollVote(PollVote pollVote)
        {
            _context.PollVotes.Add(pollVote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollVote", new { id = pollVote.VoteId }, pollVote);
        }

        // DELETE: api/PollVotesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePollVote(int id)
        {
            var pollVote = await _context.PollVotes.FindAsync(id);
            if (pollVote == null)
            {
                return NotFound();
            }

            _context.PollVotes.Remove(pollVote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PollVoteExists(int id)
        {
            return _context.PollVotes.Any(e => e.VoteId == id);
        }
    }
}
