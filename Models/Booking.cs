namespace Tour_Management.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int TOUR_ID { get; set; }
        public string TOUR_NAME { get; set; }
        public string PLACE { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
