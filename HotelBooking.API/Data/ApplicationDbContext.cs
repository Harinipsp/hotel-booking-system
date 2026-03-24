using Microsoft.EntityFrameworkCore;
namespace HotelBooking.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Room> Rooms { get; set; } // 🔥 NEW

        public DbSet<Booking> Bookings { get; set; }
    }
}