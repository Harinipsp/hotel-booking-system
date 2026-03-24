
using HotelBooking.API.Data;
using HotelBooking.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE BOOKING
        [HttpPost]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            // Check if hotel exists
            var hotel = await _context.Hotels.FindAsync(booking.HotelId);
            if (hotel == null)
                return NotFound("Hotel not found");

            // Validate Check-In / Check-Out dates
            if (booking.CheckInDate == default || booking.CheckOutDate == default)
                return BadRequest("Check-In and Check-Out dates are required");

            var nights = (booking.CheckOutDate - booking.CheckInDate).Days;
            if (nights <= 0)
                return BadRequest("Check-Out date must be after Check-In date");

            // Assign price per night and update availability
            decimal roomPrice = 0;

            if (booking.RoomType?.ToLower() == "deluxe")
            {
                if (hotel.DeluxeIsAvailable <= 0)
                    return BadRequest("No deluxe rooms available");

                hotel.DeluxeIsAvailable -= 1;
                roomPrice = hotel.DeluxePrice;
            }
            else if (booking.RoomType?.ToLower() == "standard")
            {
                if (hotel.StandardIsAvailable <= 0)
                    return BadRequest("No standard rooms available");

                hotel.StandardIsAvailable -= 1;
                roomPrice = hotel.StandardPrice;
            }
            else
            {
                return BadRequest("Invalid room type");
            }

            // Calculate total price
            booking.TotalPrice = roomPrice * nights;

            // Set initial status as Pending
            booking.Status = "Pending";

            // Save booking
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        // USER BOOKINGS
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Ok(bookings);
        }

        // HOTEL BOOKINGS (ADMIN)
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetHotelBookings(int hotelId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.HotelId == hotelId)
                .ToListAsync();

            return Ok(bookings);
        }

        // CANCEL BOOKING
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            var hotel = await _context.Hotels.FindAsync(booking.HotelId);

            if (hotel != null)
            {
                if (booking.RoomType.ToLower() == "deluxe")
                    hotel.DeluxeIsAvailable += 1;
                else if (booking.RoomType.ToLower() == "standard")
                    hotel.StandardIsAvailable += 1;
            }

            booking.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}