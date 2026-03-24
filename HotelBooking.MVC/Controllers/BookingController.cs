using Microsoft.AspNetCore.Mvc;
using HotelBooking.MVC.Services;
using HotelBooking.MVC.Models;

namespace HotelBooking.MVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApiService _apiService;

        public BookingController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // OPEN BOOK PAGE
        public async Task<IActionResult> Book(int id)
        {
            if (HttpContext.Session.GetString("Role") != "User")
                return RedirectToAction("Login", "Account");

            var hotel = await _apiService.GetAsync<Hotel>($"Hotels/{id}");

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = await _apiService.GetAsync<User>($"Users/{userId}");

            ViewBag.User = user; // MUST be set

            return View(hotel);
        }

        // CONFIRM BOOKING
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(Booking booking)
        {
            var result = await _apiService.PostAsync<Booking>("Bookings", booking);

            if (result == null)
                return Content("Booking Failed");

            return RedirectToAction("Ticket", new { id = booking.HotelId });
        }

        // SHOW TICKET
        public async Task<IActionResult> Ticket(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var bookings = await _apiService.GetAsync<List<Booking>>($"Bookings/user/{userId}")
                           ?? new List<Booking>();

            var ticket = bookings
                .Where(b => b.HotelId == id)
                .OrderByDescending(b => b.BookingId)
                .FirstOrDefault();

            return View(ticket);
        }

        // BOOKING HISTORY
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var bookings = await _apiService.GetAsync<List<Booking>>($"Bookings/user/{userId}");

            return View(bookings);
        }
    }
}