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
    public class AuthProvidersApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;

        public AuthProvidersApiController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // GET: api/AuthProvidersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthProvider>>> GetAuthProviders()
        {
            return await _context.AuthProviders.ToListAsync();
        }

        // GET: api/AuthProvidersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthProvider>> GetAuthProvider(int id)
        {
            var authProvider = await _context.AuthProviders.FindAsync(id);

            if (authProvider == null)
            {
                return NotFound();
            }

            return authProvider;
        }

        // PUT: api/AuthProvidersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthProvider(int id, AuthProvider authProvider)
        {
            if (id != authProvider.ProviderId)
            {
                return BadRequest();
            }

            _context.Entry(authProvider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthProviderExists(id))
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

        // POST: api/AuthProvidersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthProvider>> PostAuthProvider(AuthProvider authProvider)
        {
            _context.AuthProviders.Add(authProvider);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthProvider", new { id = authProvider.ProviderId }, authProvider);
        }

        // DELETE: api/AuthProvidersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthProvider(int id)
        {
            var authProvider = await _context.AuthProviders.FindAsync(id);
            if (authProvider == null)
            {
                return NotFound();
            }

            _context.AuthProviders.Remove(authProvider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthProviderExists(int id)
        {
            return _context.AuthProviders.Any(e => e.ProviderId == id);
        }
    }
}
