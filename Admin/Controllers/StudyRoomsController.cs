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
    public class StudyRoomsController : Controller
    {
        private readonly StudySpaceDbContext _context;

        public StudyRoomsController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: StudyRooms
        public async Task<IActionResult> Index()
        {
            var studySpaceDbContext = _context.StudyRooms.Include(s => s.CreatedByNavigation);
            return View(await studySpaceDbContext.ToListAsync());
        }

        // GET: StudyRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyRoom = await _context.StudyRooms
                .Include(s => s.CreatedByNavigation)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (studyRoom == null)
            {
                return NotFound();
            }

            return View(studyRoom);
        }

        // GET: StudyRooms/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: StudyRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomName,IsPrivate,AccessCode,CreatedBy,CreatedAt")] StudyRoom studyRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studyRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.Users, "UserId", "UserId", studyRoom.CreatedBy);
            return View(studyRoom);
        }

        // GET: StudyRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyRoom = await _context.StudyRooms.FindAsync(id);
            if (studyRoom == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.Users, "UserId", "UserId", studyRoom.CreatedBy);
            return View(studyRoom);
        }

        // POST: StudyRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomName,IsPrivate,AccessCode,CreatedBy,CreatedAt")] StudyRoom studyRoom)
        {
            if (id != studyRoom.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studyRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyRoomExists(studyRoom.RoomId))
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
            ViewData["CreatedBy"] = new SelectList(_context.Users, "UserId", "UserId", studyRoom.CreatedBy);
            return View(studyRoom);
        }

        // GET: StudyRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyRoom = await _context.StudyRooms
                .Include(s => s.CreatedByNavigation)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (studyRoom == null)
            {
                return NotFound();
            }

            return View(studyRoom);
        }

        // POST: StudyRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studyRoom = await _context.StudyRooms.FindAsync(id);
            if (studyRoom != null)
            {
                _context.StudyRooms.Remove(studyRoom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyRoomExists(int id)
        {
            return _context.StudyRooms.Any(e => e.RoomId == id);
        }
    }
}
