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
    public class AuthProvidersController : Controller
    {
        private readonly StudySpaceDbContext _context;

        public AuthProvidersController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: AuthProviders
        public async Task<IActionResult> Index()
        {
            var studySpaceDbContext = _context.AuthProviders.Include(a => a.User);
            return View(await studySpaceDbContext.ToListAsync());
        }

        // GET: AuthProviders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authProvider = await _context.AuthProviders
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ProviderId == id);
            if (authProvider == null)
            {
                return NotFound();
            }

            return View(authProvider);
        }

        // GET: AuthProviders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: AuthProviders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProviderId,ProviderName,ProviderKey,UserId")] AuthProvider authProvider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(authProvider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", authProvider.UserId);
            return View(authProvider);
        }

        // GET: AuthProviders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authProvider = await _context.AuthProviders.FindAsync(id);
            if (authProvider == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", authProvider.UserId);
            return View(authProvider);
        }

        // POST: AuthProviders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProviderId,ProviderName,ProviderKey,UserId")] AuthProvider authProvider)
        {
            if (id != authProvider.ProviderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authProvider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthProviderExists(authProvider.ProviderId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", authProvider.UserId);
            return View(authProvider);
        }

        // GET: AuthProviders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authProvider = await _context.AuthProviders
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ProviderId == id);
            if (authProvider == null)
            {
                return NotFound();
            }

            return View(authProvider);
        }

        // POST: AuthProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authProvider = await _context.AuthProviders.FindAsync(id);
            if (authProvider != null)
            {
                _context.AuthProviders.Remove(authProvider);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthProviderExists(int id)
        {
            return _context.AuthProviders.Any(e => e.ProviderId == id);
        }
    }
}
