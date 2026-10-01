using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studySpaceWebApp.Models;
using studySpaceWebApp.Models.DTOs;

namespace studySpaceWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly StudySpaceDbContext _context;

        public UsersApiController(StudySpaceDbContext context)
        {
            _context = context;
        }

        // ✅ Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // ✅ Get user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return user;
        }

        // ✅ Signup (React sends: fullName, email, password)
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserSignupDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                return BadRequest(new { message = "Email and password are required." });

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict(new { message = "Email already exists." });

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.Password,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, new
            {
                user.UserId,
                user.FullName,
                user.Email
            });
        }

        // ✅ Login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserSignupDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(new
            {
                user.UserId,
                user.FullName,
                user.Email,
                message = "Login successful"
            });
        }

        // ✅ Update user
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
                return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // ✅ Delete user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return Ok(); // Do not reveal whether email exists

            // Example: generate a fake reset token (in real apps, generate a secure token and save it)
            var resetToken = Guid.NewGuid().ToString();

            // Simulate sending email (replace this with actual SMTP logic)
            Console.WriteLine($"[SIMULATED EMAIL] Send reset link to {email}: https://yourapp/reset-password?token={resetToken}");

            // TODO: Save the token in DB if implementing full password reset flow

            return Ok("Password reset link sent.");
        }

    }
}
