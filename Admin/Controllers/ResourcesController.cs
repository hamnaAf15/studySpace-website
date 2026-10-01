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
    public class ResourcesController : Controller
    {
        private readonly StudySpaceDbContext _context;

        public ResourcesController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: Resources
        public async Task<IActionResult> Index()
        {
            var studySpaceDbContext = _context.Resources.Include(r => r.UploadedByNavigation);
            return View(await studySpaceDbContext.ToListAsync());
        }

        // GET: Resources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources
                .Include(r => r.UploadedByNavigation)
                .FirstOrDefaultAsync(m => m.ResourceId == id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // GET: Resources/Create
        public IActionResult Create()
        {
            ViewData["UploadedBy"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Resources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResourceId,Title,FileType,FileUrl,UploadedBy,UploadedAt")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UploadedBy"] = new SelectList(_context.Users, "UserId", "UserId", resource.UploadedBy);
            return View(resource);
        }

        // GET: Resources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            ViewData["UploadedBy"] = new SelectList(_context.Users, "UserId", "UserId", resource.UploadedBy);
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResourceId,Title,FileType,FileUrl,UploadedBy,UploadedAt")] Resource resource)
        {
            if (id != resource.ResourceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceExists(resource.ResourceId))
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
            ViewData["UploadedBy"] = new SelectList(_context.Users, "UserId", "UserId", resource.UploadedBy);
            return View(resource);
        }

        // GET: Resources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources
                .Include(r => r.UploadedByNavigation)
                .FirstOrDefaultAsync(m => m.ResourceId == id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource != null)
            {
                _context.Resources.Remove(resource);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.ResourceId == id);
        }
    }
}
