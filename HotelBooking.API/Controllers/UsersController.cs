using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using HotelBooking.API.Data;
using HotelBooking.API.Models;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // REGISTER USER
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == user.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            // 🔥 Convert Password → Hash
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // 🔥 Remove plain password
            user.Password = null;

            // Role default
            user.Role = "User";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // LOGIN USER / ADMIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            // 🔥 ADMIN LOGIN (HARDCODED)
            if (login.Email == "admin@gmail.com" && login.Password == "1234")
            {
                return Ok(new
                {
                    role = "Admin",
                    userId = 0,
                    name = "Admin",
                    email = "admin@gmail.com"
                });
            }

            // 🔥 USER LOGIN FROM DB
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email");
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);

            if (!isValid)
            {
                return Unauthorized("Invalid password");
            }

            return Ok(new
            {
                role = user.Role,
                userId = user.UserId,
                name = user.Name,
                email = user.Email
            });
        }

        // GET USER DETAILS
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}