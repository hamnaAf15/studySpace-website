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
    public class NotesHistoriesApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;

        public NotesHistoriesApiController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: api/NotesHistoriesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotesHistory>>> GetNotesHistories()
        {
            return await _context.NotesHistories.ToListAsync();
        }

        // GET: api/NotesHistoriesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotesHistory>> GetNotesHistory(int id)
        {
            var notesHistory = await _context.NotesHistories.FindAsync(id);

            if (notesHistory == null)
            {
                return NotFound();
            }

            return notesHistory;
        }

        // PUT: api/NotesHistoriesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotesHistory(int id, NotesHistory notesHistory)
        {
            if (id != notesHistory.HistoryId)
            {
                return BadRequest();
            }

            _context.Entry(notesHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesHistoryExists(id))
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

        // POST: api/NotesHistoriesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NotesHistory>> PostNotesHistory(NotesHistory notesHistory)
        {
            _context.NotesHistories.Add(notesHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotesHistory", new { id = notesHistory.HistoryId }, notesHistory);
        }

        // DELETE: api/NotesHistoriesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotesHistory(int id)
        {
            var notesHistory = await _context.NotesHistories.FindAsync(id);
            if (notesHistory == null)
            {
                return NotFound();
            }

            _context.NotesHistories.Remove(notesHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotesHistoryExists(int id)
        {
            return _context.NotesHistories.Any(e => e.HistoryId == id);
        }
    }
}
