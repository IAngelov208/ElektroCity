using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElektroCity.Data;
using ElektroCity.Models;

namespace ElektroCity.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Зареждане на залата и заетите места (GET)
        public async Task<IActionResult> ChooseSeats(int id)
        {
            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (screening == null)
            {
                screening = await _context.Screenings.Include(s => s.Movie).FirstOrDefaultAsync();
            }

            if (screening == null)
            {
                var movie = await _context.Movies.FirstOrDefaultAsync();

                if (movie == null)
                {
                    return Content("Моля, първо добавете филм от панела /Movies!");
                }

                screening = new Screening
                {
                    HallName = "Зала 1 (IMAX)",
                    Showtime = DateTime.Now.AddHours(2),
                    TicketPrice = 14.50m,
                    MovieId = movie.Id
                };

                _context.Screenings.Add(screening);
                await _context.SaveChangesAsync();
            }

            var occupiedSeatsFromDb = await _context.Reservations
                .Where(r => r.ScreeningId == screening.Id)
                .Select(r => r.ChosenSeats)
                .ToListAsync();

            var occupiedSeatsList = new List<string>();
            foreach (var seatString in occupiedSeatsFromDb)
            {
                if (!string.IsNullOrEmpty(seatString))
                {
                    var splitSeats = seatString.Split(',').Select(s => s.Trim());
                    occupiedSeatsList.AddRange(splitSeats);
                }
            }

            ViewBag.OccupiedSeats = occupiedSeatsList;

            return View(screening);
        }

        // 2. Записване на резервацията в базата (POST)
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> BookSeats(int screeningId, string seats, decimal totalPrice)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, isNotLogged = true, message = "Трябва да сте влезли в акаунта си, за да направите резервация!" });
            }

            if (string.IsNullOrEmpty(seats))
            {
                return Json(new { success = false, message = "Не сте избрали места!" });
            }

            var screeningExists = await _context.Screenings.AnyAsync(s => s.Id == screeningId);
            if (!screeningExists)
            {
                return Json(new { success = false, message = "Избраната прожекция не съществува в базата данни." });
            }

            try
            {
                var reservation = new Reservation
                {
                    ScreeningId = screeningId,
                    UserEmail = userEmail,
                    ChosenSeats = seats,
                    TotalPrice = totalPrice,
                    ReservedAt = DateTime.Now
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Резервацията е успешна! Местата Ви са запазени." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Възникна системна грешка: {ex.Message}" });
            }
        }

        // 3. Списък с резервациите на потребителя (GET)
        public async Task<IActionResult> MyReservations()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var reservations = await _context.Reservations
                .Include(r => r.Screening)
                .ThenInclude(s => s.Movie)
                .Where(r => r.UserEmail == userEmail)
                .OrderByDescending(r => r.Screening.Showtime)
                .ToListAsync();

            return View(reservations);
        }
    }
}