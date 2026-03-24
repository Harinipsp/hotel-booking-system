namespace HotelBooking.MVC.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public int DeluxeCapacity { get; set; }
        public int DeluxeIsAvailable { get; set; }

        public int StandardCapacity { get; set; }
        public int StandardIsAvailable { get; set; }

        public decimal DeluxePrice { get; set; }
        public decimal StandardPrice { get; set; }
    }
}