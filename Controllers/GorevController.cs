using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeTasksApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTasksApp.Controllers
{
    public class GorevController : Controller
    {
        private readonly UygulamaDbContext _context;

        public GorevController(UygulamaDbContext context)
        {
            _context = context;
        }

        // GÖREV OLUŞTUR (GET)
        public IActionResult Create()
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var currentUser = _context.Users
                .Include(u => u.Household)
                .FirstOrDefault(u => u.Id == currentUserId);

            var users = _context.Users
                .Where(u => u.HouseholdId == currentUser.HouseholdId)
                .ToList();

            ViewBag.KullaniciListesi = new SelectList(users, "Id", "Name");
            return View();
        }

        // GÖREV OLUŞTUR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Gorev gorev)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                gorev.TamamlandiMi = false;
                _context.Gorevler.Add(gorev);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var currentUser = _context.Users
                .Include(u => u.Household)
                .FirstOrDefault(u => u.Id == currentUserId);

            var users = _context.Users
                .Where(u => u.HouseholdId == currentUser.HouseholdId)
                .ToList();

            ViewBag.KullaniciListesi = new SelectList(users, "Id", "Name");
            return View(gorev);
        }
        // GÖREVLERİ GÜNCELLE (GET)
        public IActionResult Edit(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var gorev = _context.Gorevler.FirstOrDefault(g => g.Id == id);
            if (gorev == null)
                return NotFound();

            var currentUser = _context.Users
                .Include(u => u.Household)
                .FirstOrDefault(u => u.Id == currentUserId);

            var users = _context.Users
                .Where(u => u.HouseholdId == currentUser.HouseholdId)
                .ToList();

            ViewBag.KullaniciListesi = new SelectList(users, "Id", "Name", gorev.AssignedUserId);
            return View(gorev);
        }

        // GÖREVLERİ GÜNCELLE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Gorev gorev)
        {
            if (ModelState.IsValid)
            {
                var mevcutGorev = _context.Gorevler.FirstOrDefault(g => g.Id == gorev.Id);
                if (mevcutGorev == null)
                    return NotFound();

                mevcutGorev.Baslik = gorev.Baslik;
                mevcutGorev.Aciklama = gorev.Aciklama;
                mevcutGorev.SonTarih = gorev.SonTarih;
                mevcutGorev.AssignedUserId = gorev.AssignedUserId;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var users = _context.Users.ToList();
            ViewBag.KullaniciListesi = new SelectList(users, "Id", "Name", gorev.AssignedUserId);
            return View(gorev);
        }

        // GÖREVLERİ LİSTELE
        public IActionResult Index()
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var currentUser = _context.Users
                .Include(u => u.Household)
                .FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var gorevler = _context.Gorevler
                .Include(g => g.AssignedUser)
                .Include(g => g.CompletedByUser)
                .Where(g => g.AssignedUser.HouseholdId == currentUser.HouseholdId)
                .ToList();

            ViewBag.CurrentUserId = currentUserId;

            return View(gorevler);
        }
        // GÖREV SİL (GET)
        public IActionResult Delete(int id)
        {
            var gorev = _context.Gorevler
                .Include(g => g.AssignedUser)
                .FirstOrDefault(g => g.Id == id);

            if (gorev == null)
                return NotFound();

            return View(gorev);
        }

        // GÖREV SİL (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var gorev = _context.Gorevler.Find(id);
            if (gorev != null)
            {
                _context.Gorevler.Remove(gorev);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // GÖREVİ TAMAMLAMA
        [HttpPost]
        public IActionResult Tamamla(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var gorev = _context.Gorevler.FirstOrDefault(g => g.Id == id);
            if (gorev != null && !gorev.TamamlandiMi)
            {
                gorev.TamamlandiMi = true;
                gorev.CompletedByUserId = currentUserId;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
