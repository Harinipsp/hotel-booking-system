
using HotelBooking.API.Data;
using HotelBooking.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HotelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            return await _context.Hotels.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
                return NotFound();

            return hotel;
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> CreateHotel(Hotel hotel)
        {
            // Initialize availability
            hotel.DeluxeIsAvailable = hotel.DeluxeCapacity;
            hotel.StandardIsAvailable = hotel.StandardCapacity;

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.HotelId }, hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotel hotel)
        {
            if (id != hotel.HotelId)
                return BadRequest();

            var existingHotel = await _context.Hotels.FindAsync(id);

            if (existingHotel == null)
                return NotFound();

            existingHotel.HotelName = hotel.HotelName;
            existingHotel.Location = hotel.Location;
            existingHotel.Description = hotel.Description;

            existingHotel.DeluxePrice = hotel.DeluxePrice;
            existingHotel.StandardPrice = hotel.StandardPrice;

            existingHotel.DeluxeCapacity = hotel.DeluxeCapacity;
            existingHotel.StandardCapacity = hotel.StandardCapacity;

            // reset availability
            existingHotel.DeluxeIsAvailable = hotel.DeluxeCapacity;
            existingHotel.StandardIsAvailable = hotel.StandardCapacity;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
                return NotFound();

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}