using Microsoft.AspNetCore.Mvc;
using studySpaceWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace studySpaceWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly StudySpaceDbContext _context;

        public LoginController(StudySpaceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (admin != null)
            {
                HttpContext.Session.SetString("AdminName", admin.Name);
                return RedirectToAction("Index", "AdminDashboard");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
