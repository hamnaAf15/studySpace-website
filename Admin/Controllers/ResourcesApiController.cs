using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studySpaceWebApp.Models;

namespace studySpaceWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ResourcesApiController(StudySpaceDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/ResourcesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        // POST: api/ResourcesApi/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadResource(IFormFile file, [FromForm] string title, [FromForm] string type)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var resource = new Resource
            {
                Title = title,
                FileType = type,
                FileUrl = $"/uploads/{uniqueFileName}",
                UploadedAt = DateTime.Now
            };

            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return Ok(resource);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
                return NotFound();

            var fullPath = Path.Combine(_env.WebRootPath, resource.FileUrl.TrimStart('/'));

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
