public class Room
{
    public int RoomId { get; set; }

    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }

    public string RoomNumber { get; set; }

    public string RoomType { get; set; } // Deluxe / Standard

    public bool IsAvailable { get; set; } = true;
}