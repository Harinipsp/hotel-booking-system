public class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public int HotelId { get; set; }
    public decimal TotalPrice { get; set; }
    public Room Room { get; set; }
    public string RoomType { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public string Status { get; set; } = "Confirmed";
}