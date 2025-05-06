using HomeTasksApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HomeTasksApp.Controllers
{
    public class HouseholdAdminController : BaseController
    {
        private readonly UygulamaDbContext _context;

        public HouseholdAdminController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null) return RedirectToAction("Login", "Account");

            var user = _context.Users
                .Include(u => u.Household)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null || !user.IsHouseholdAdmin) return RedirectToAction("Index", "Home");

            var usersInHousehold = _context.Users
                .Where(u => u.HouseholdId == user.HouseholdId)
                .ToList();

            var tasks = _context.Gorevler
                .Where(g => g.AssignedUser.HouseholdId == user.HouseholdId)
                .Include(g => g.AssignedUser)
                .ToList();

            ViewBag.HouseholdUsers = usersInHousehold;
            ViewBag.HouseholdTasks = tasks;

            return View();
        }
    }

}
