public class Hotel
{
    public int HotelId { get; set; }

    public string HotelName { get; set; }

    public string Location { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    // Prices
    public decimal DeluxePrice { get; set; }
    public decimal StandardPrice { get; set; }

    // Capacity
    public int DeluxeCapacity { get; set; }
    public int StandardCapacity { get; set; }

    // Availability
    public int DeluxeIsAvailable { get; set; }
    public int StandardIsAvailable { get; set; }

    // Navigation
    public List<Room> Rooms { get; set; }
}