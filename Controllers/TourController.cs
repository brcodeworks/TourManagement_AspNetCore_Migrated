using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tour_Management.Models;

namespace Tour_Management.Controllers
{
    public class TourController : Controller
    {
        private readonly string _cs;

        public TourController(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult DisplayTours()
        {
            List<Tour> tours = new List<Tour>();
            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                using var cmd = new SqlCommand("SELECT TOUR_ID, TOUR_NAME, DAYS, LOCATIONS, PRICE, pic FROM Tour", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tours.Add(new Tour
                    {
                        TOUR_ID = Convert.ToInt32(reader["TOUR_ID"]),
                        TOUR_NAME = reader["TOUR_NAME"].ToString(),
                        DAYS = Convert.ToInt32(reader["DAYS"]),
                        LOCATIONS = reader["LOCATIONS"].ToString(),
                        PRICE = Convert.ToDecimal(reader["PRICE"]),
                        pic = reader["pic"].ToString()
                    });
                }
            }
            return View(tours);
        }
    }
}
