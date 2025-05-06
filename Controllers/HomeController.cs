using Microsoft.AspNetCore.Mvc;
using HomeTasksApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace HomeTasksApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UygulamaDbContext _context;

        public HomeController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            var userId = GetCurrentUserId();
            var user = _context.Users
                .Include(u => u.Household)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login", "Account");

            // Bu kullanıcıya atanmış görevler
            var gorevler = _context.Gorevler
                .Include(g => g.AssignedUser)
                .Where(g => g.AssignedUserId == userId)
                .ToList();

            ViewBag.UserName = user.Name;
            ViewBag.Gorevler = gorevler;

            return View();
        }

    }
}
