using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeTasksApp.Models;
using HomeTasksApp.ViewModels;
using Microsoft.AspNetCore.Http;

namespace HomeTasksApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AccountController(UygulamaDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var household = await _context.Households
                .FirstOrDefaultAsync(h => h.Name == model.HouseholdName);

            if (household == null)
            {
                household = new Household { Name = model.HouseholdName };
                _context.Households.Add(household);
                await _context.SaveChangesAsync();
            }

            bool isFirstUser = !_context.Users.Any();

            var user = new User
            {
                Name = model.Name,
                Password = model.Password,
                IsAdmin = isFirstUser,                  // sadece ilk kullanıcı admin
                IsHouseholdAdmin = !isFirstUser,        // diğeri varsa reisi o olur
                Household = household
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToRolePanel(user);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .Include(u => u.Household)
                .FirstOrDefaultAsync(u => u.Name == model.Name && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToRolePanel(user);
        }

        private IActionResult RedirectToRolePanel(User user)
        {
            if (user.IsAdmin)
                return RedirectToAction("Index", "Admin");

            if (user.IsHouseholdAdmin)
                return RedirectToAction("Dashboard", "HouseholdAdmin");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
