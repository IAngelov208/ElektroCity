using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElektroCity.Data;
using System.Diagnostics;
using ElektroCity.Models;

namespace ElektroCity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Взимаме всички филми от базата данни и ги пращаме към началната страница
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}