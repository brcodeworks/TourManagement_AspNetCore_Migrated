using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tour_Management.Models;

namespace Tour_Management.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _cs;

        public AdminController(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        #region Admin Login & Profile

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == "admin@gmail.com" && password == "admin")
            {
                HttpContext.Session.SetString("AdminEmail", email);
                return RedirectToAction("Profile");
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            return View();
        }

        #endregion

        #region Add Tour

        [HttpGet]
        public IActionResult AddTour()
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTour(string tour_name, string place, string days, string locations, string price, string tour_info, IFormFile pic)
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            string fileName = null;
            if (pic != null && pic.Length > 0)
            {
                fileName = Path.GetFileName(pic.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Tour_pics", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await pic.CopyToAsync(stream);
                }
            }

            using (var conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                string insertQuery = @"INSERT INTO Tour 
                    (TOUR_NAME, PLACE, DAYS, PRICE, LOCATIONS, TOUR_INFO, pic) 
                    VALUES (@TOUR_NAME, @PLACE, @DAYS, @PRICE, @LOCATIONS, @TOUR_INFO, @pic)";

                using var cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@TOUR_NAME", tour_name);
                cmd.Parameters.AddWithValue("@PLACE", place);
                cmd.Parameters.AddWithValue("@DAYS", days);
                cmd.Parameters.AddWithValue("@PRICE", price);
                cmd.Parameters.AddWithValue("@LOCATIONS", locations);
                cmd.Parameters.AddWithValue("@TOUR_INFO", tour_info);
                cmd.Parameters.AddWithValue("@pic", fileName ?? "");

                await cmd.ExecuteNonQueryAsync();
            }

            ViewBag.Message = "Tour added successfully!";
            return View();
        }

        #endregion

        #region Tour CRUD (Admin Only)

        [HttpGet]
        public IActionResult TourCrud()
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            List<Tour> tours = new();
            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                using var cmd = new SqlCommand("SELECT * FROM Tour", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tours.Add(new Tour
                    {
                        TOUR_ID = Convert.ToInt32(reader["TOUR_ID"]),
                        TOUR_NAME = reader["TOUR_NAME"].ToString(),
                        PLACE = reader["PLACE"].ToString(),
                        DAYS = Convert.ToInt32(reader["DAYS"]),
                        PRICE = Convert.ToDecimal(reader["PRICE"]),
                        LOCATIONS = reader["LOCATIONS"].ToString(),
                        TOUR_INFO = reader["TOUR_INFO"].ToString(),
                        pic = reader["pic"].ToString()
                    });
                }
            }
            return View(tours);
        }

        [HttpGet]
        public IActionResult EditTour(int id)
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            Tour tour = null;
            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                using var cmd = new SqlCommand("SELECT * FROM Tour WHERE TOUR_ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    tour = new Tour
                    {
                        TOUR_ID = Convert.ToInt32(reader["TOUR_ID"]),
                        TOUR_NAME = reader["TOUR_NAME"].ToString(),
                        PLACE = reader["PLACE"].ToString(),
                        DAYS = Convert.ToInt32(reader["DAYS"]),
                        PRICE = Convert.ToDecimal(reader["PRICE"]),
                        LOCATIONS = reader["LOCATIONS"].ToString(),
                        TOUR_INFO = reader["TOUR_INFO"].ToString(),
                        pic = reader["pic"].ToString()
                    };
                }
            }
            return View(tour);
        }

        [HttpPost]
        public async Task<IActionResult> EditTour(Tour tour, IFormFile newPic)
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            string fileName = tour.pic;
            if (newPic != null && newPic.Length > 0)
            {
                fileName = Path.GetFileName(newPic.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Tour_pics", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await newPic.CopyToAsync(stream);
                }
            }

            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                string updateQuery = @"UPDATE Tour 
                                       SET TOUR_NAME=@TOUR_NAME, PLACE=@PLACE, DAYS=@DAYS, PRICE=@PRICE,
                                           LOCATIONS=@LOCATIONS, TOUR_INFO=@TOUR_INFO, pic=@pic
                                       WHERE TOUR_ID=@TOUR_ID";
                using var cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@TOUR_ID", tour.TOUR_ID);
                cmd.Parameters.AddWithValue("@TOUR_NAME", tour.TOUR_NAME);
                cmd.Parameters.AddWithValue("@PLACE", tour.PLACE);
                cmd.Parameters.AddWithValue("@DAYS", tour.DAYS);
                cmd.Parameters.AddWithValue("@PRICE", tour.PRICE);
                cmd.Parameters.AddWithValue("@LOCATIONS", tour.LOCATIONS);
                cmd.Parameters.AddWithValue("@TOUR_INFO", tour.TOUR_INFO);
                cmd.Parameters.AddWithValue("@pic", fileName ?? "");

                await cmd.ExecuteNonQueryAsync();
            }

            return RedirectToAction("TourCrud");
        }

        [HttpPost]
        public IActionResult DeleteTour(int id)
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                using var cmd = new SqlCommand("DELETE FROM Tour WHERE TOUR_ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("TourCrud");
        }

        #endregion

        #region All Bookings

        [HttpGet]
        public IActionResult AllBooking()
        {
            var admin = HttpContext.Session.GetString("AdminEmail");
            if (admin == null) return RedirectToAction("Login");

            List<Booking> bookings = new List<Booking>();
            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                using var cmd = new SqlCommand("SELECT TOUR_ID, TOUR_NAME, PLACE, Email, FirstName FROM booking", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bookings.Add(new Booking
                    {
                        TOUR_ID = Convert.ToInt32(reader["TOUR_ID"]),
                        TOUR_NAME = reader["TOUR_NAME"].ToString(),
                        PLACE = reader["PLACE"].ToString(),
                        Email = reader["Email"].ToString(),
                        FirstName = reader["FirstName"].ToString()
                    });
                }
            }

            return View(bookings);
        }

        #endregion

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Login");
        }
    }
}