using Microsoft.AspNetCore.Mvc;
using HotelBooking.MVC.Models;
using HotelBooking.MVC.Services;

namespace HotelBooking.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }

        // VIEW HOTELS
        public async Task<IActionResult> Hotels()
        {
            var hotels = await _apiService.GetAsync<List<Hotel>>("Hotels");
            return View(hotels);
        }

        // VIEW BOOKINGS OF HOTEL
        public async Task<IActionResult> Bookings(int hotelId)
        {
            var bookings = await _apiService.GetAsync<List<Booking>>($"Bookings/hotel/{hotelId}");
            ViewBag.HotelId = hotelId;
            return View(bookings);
        }

        // CREATE HOTEL
        public IActionResult CreateHotel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel(Hotel hotel)
        {
            await _apiService.PostAsync<object>("Hotels", hotel);
            return RedirectToAction("Hotels");
        }

        // EDIT HOTEL
        public async Task<IActionResult> EditHotel(int id)
        {
            var hotel = await _apiService.GetAsync<Hotel>($"Hotels/{id}");
            return View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> EditHotel(Hotel hotel)
        {
            await _apiService.PutAsync<object>($"Hotels/{hotel.HotelId}", hotel);
            return RedirectToAction("Hotels");
        }

        // DELETE HOTEL
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _apiService.DeleteAsync($"Hotels/{id}");
            return RedirectToAction("Hotels");
        }
    }
}