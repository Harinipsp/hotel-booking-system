using Microsoft.AspNetCore.Mvc;
using HotelBooking.MVC.Services;
using HotelBooking.MVC.Models;

namespace HotelBooking.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // HOMEPAGE
        public async Task<IActionResult> Index(string search, string location)
        {
            var hotels = await _apiService.GetAsync<List<Hotel>>("Hotels") ?? new List<Hotel>();

            // SEARCH
            if (!string.IsNullOrWhiteSpace(search))
            {
                hotels = hotels
                    .Where(h => h.HotelName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // FILTER LOCATION
            if (!string.IsNullOrWhiteSpace(location))
            {
                hotels = hotels
                    .Where(h => h.Location.Equals(location, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(hotels);
        }

        // HOTEL DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _apiService.GetAsync<Hotel>($"Hotels/{id}");
            return View(hotel);
        }
    }
}