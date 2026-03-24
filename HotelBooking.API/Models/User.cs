using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int UserId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    [NotMapped] // 🔥 VERY IMPORTANT
    public string Password { get; set; }

    public string MobileNumber { get; set; }

    public string Role { get; set; } = "User";
}