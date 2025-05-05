using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeTasksApp.Models;
using System.Linq;

namespace HomeTasksApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly UygulamaDbContext _context;

        public AdminController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            var userId = GetCurrentUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null || !user.IsAdmin)
                return RedirectToAction("Index", "Home");

            var households = _context.Households
                .Include(h => h.Users)
                .ToList();

            return View(households);
        }
    }
}
