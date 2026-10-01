using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using studySpaceWebApp.Models;

namespace studySpaceWebApp.Controllers
{
    public class RoomMembersController : Controller
    {
        private readonly StudySpaceDbContext _context;

        public RoomMembersController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: RoomMembers
        public async Task<IActionResult> Index()
        {
            var studySpaceDbContext = _context.RoomMembers.Include(r => r.Room).Include(r => r.User);
            return View(await studySpaceDbContext.ToListAsync());
        }

        // GET: RoomMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomMember = await _context.RoomMembers
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (roomMember == null)
            {
                return NotFound();
            }

            return View(roomMember);
        }

        // GET: RoomMembers/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.StudyRooms, "RoomId", "RoomId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: RoomMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,RoomId,UserId,JoinedAt")] RoomMember roomMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.StudyRooms, "RoomId", "RoomId", roomMember.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", roomMember.UserId);
            return View(roomMember);
        }

        // GET: RoomMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomMember = await _context.RoomMembers.FindAsync(id);
            if (roomMember == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.StudyRooms, "RoomId", "RoomId", roomMember.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", roomMember.UserId);
            return View(roomMember);
        }

        // POST: RoomMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,RoomId,UserId,JoinedAt")] RoomMember roomMember)
        {
            if (id != roomMember.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomMemberExists(roomMember.MemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.StudyRooms, "RoomId", "RoomId", roomMember.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", roomMember.UserId);
            return View(roomMember);
        }

        // GET: RoomMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomMember = await _context.RoomMembers
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (roomMember == null)
            {
                return NotFound();
            }

            return View(roomMember);
        }

        // POST: RoomMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomMember = await _context.RoomMembers.FindAsync(id);
            if (roomMember != null)
            {
                _context.RoomMembers.Remove(roomMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomMemberExists(int id)
        {
            return _context.RoomMembers.Any(e => e.MemberId == id);
        }
    }
}
