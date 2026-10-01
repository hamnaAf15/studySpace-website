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
    public class RoomMembersApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;

        public RoomMembersApiController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: api/RoomMembersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomMember>>> GetRoomMembers()
        {
            return await _context.RoomMembers.ToListAsync();
        }

        // GET: api/RoomMembersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomMember>> GetRoomMember(int id)
        {
            var roomMember = await _context.RoomMembers.FindAsync(id);

            if (roomMember == null)
            {
                return NotFound();
            }

            return roomMember;
        }

        // PUT: api/RoomMembersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomMember(int id, RoomMember roomMember)
        {
            if (id != roomMember.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(roomMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomMemberExists(id))
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

        // POST: api/RoomMembersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomMember>> PostRoomMember(RoomMember roomMember)
        {
            _context.RoomMembers.Add(roomMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomMember", new { id = roomMember.MemberId }, roomMember);
        }

        // DELETE: api/RoomMembersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomMember(int id)
        {
            var roomMember = await _context.RoomMembers.FindAsync(id);
            if (roomMember == null)
            {
                return NotFound();
            }

            _context.RoomMembers.Remove(roomMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomMemberExists(int id)
        {
            return _context.RoomMembers.Any(e => e.MemberId == id);
        }
        // DELETE: api/RoomMembersApi/leave?roomId=1&userId=5
        [HttpDelete("leave")]
        public async Task<IActionResult> LeaveRoom(int roomId, int userId)
        {
            var membership = await _context.RoomMembers
                .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId);

            if (membership == null)
                return NotFound();

            _context.RoomMembers.Remove(membership);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
