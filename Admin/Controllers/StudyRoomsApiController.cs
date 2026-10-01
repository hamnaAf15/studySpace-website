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
    public class StudyRoomsApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;

        public StudyRoomsApiController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: api/StudyRoomsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudyRoom>>> GetStudyRooms()
        {
            return await _context.StudyRooms.ToListAsync();
        }

        // GET: api/StudyRoomsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudyRoom>> GetStudyRoom(int id)
        {
            var studyRoom = await _context.StudyRooms.FindAsync(id);

            if (studyRoom == null)
            {
                return NotFound();
            }

            return studyRoom;
        }

        // PUT: api/StudyRoomsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudyRoom(int id, StudyRoom studyRoom)
        {
            if (id != studyRoom.RoomId)
            {
                return BadRequest();
            }

            _context.Entry(studyRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyRoomExists(id))
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

        // POST: api/StudyRoomsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudyRoom>> PostStudyRoom(StudyRoom studyRoom)
        {
            _context.StudyRooms.Add(studyRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudyRoom", new { id = studyRoom.RoomId }, studyRoom);
        }

        // DELETE: api/StudyRoomsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyRoom(int id)
        {
            var studyRoom = await _context.StudyRooms.FindAsync(id);
            if (studyRoom == null)
            {
                return NotFound();
            }

            _context.StudyRooms.Remove(studyRoom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudyRoomExists(int id)
        {
            return _context.StudyRooms.Any(e => e.RoomId == id);
        }
    }
}
