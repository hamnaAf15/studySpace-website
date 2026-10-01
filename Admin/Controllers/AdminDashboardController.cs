using Microsoft.AspNetCore.Mvc;
using studySpaceWebApp.Models;
using studySpaceWebApp.Models.ViewModels;
using System.Linq;

namespace studySpaceWebApp.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly StudySpaceDbContext _context;

        public AdminDashboardController(StudySpaceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalUsers = _context.Users.Count(),
                TotalRooms = _context.StudyRooms.Count(),
                TotalPolls = _context.Polls.Count(),
                ActiveMembers = _context.RoomMembers.Count() // Assumes IsActive column
            };

            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            return View(viewModel);
        }
    }
}
