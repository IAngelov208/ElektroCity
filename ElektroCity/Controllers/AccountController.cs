using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElektroCity.Data;
using ElektroCity.Models;

namespace ElektroCity.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Страница за Регистрация (Изглед)
        public IActionResult Register() => View();

        // Процес на Регистрация
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Този имейл вече е регистриран!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        // Страница за Вход (Изглед)
        public IActionResult Login() => View();

        // Процес на Вход
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Записваме потребителя в сесията на браузъра
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.FullName);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Грешен имейл или парола!");
            return View();
        }

        // Изход
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}