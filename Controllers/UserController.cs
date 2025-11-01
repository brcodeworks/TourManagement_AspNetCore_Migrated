using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tour_Management.Models;

namespace Tour_Management.Controllers
{
    public class UserController : Controller
    {
        private readonly string _cs;
        public UserController(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            var users = new List<UserInfo>();
            using (var conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                using var cmd = new SqlCommand("SELECT Email, FirstName, LastName, Gender, Password, City FROM UserInfo", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new UserInfo
                    {
                        Email = reader["Email"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Password = reader["Password"].ToString(),
                        City = reader["City"].ToString()
                    });
                }
            }
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            UserInfo user = null;
            using (var conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                using var cmd = new SqlCommand("SELECT * FROM UserInfo WHERE Email=@Email", conn);
                cmd.Parameters.AddWithValue("@Email", id);
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    user = new UserInfo
                    {
                        Email = reader["Email"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Password = reader["Password"].ToString(),
                        City = reader["City"].ToString()
                    };
                }
            }

            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserInfo model)
        {
            if (!ModelState.IsValid) return View(model);

            using (var conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                var sql = @"UPDATE UserInfo SET 
                            FirstName=@FirstName, LastName=@LastName, Gender=@Gender, Password=@Password, City=@City
                            WHERE Email=@Email";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                cmd.Parameters.AddWithValue("@LastName", model.LastName);
                cmd.Parameters.AddWithValue("@Gender", model.Gender);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@City", model.City);
                await cmd.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            using (var conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                using var cmd = new SqlCommand("DELETE FROM UserInfo WHERE Email=@Email", conn);
                cmd.Parameters.AddWithValue("@Email", id);
                await cmd.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult MyBooking()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login");

            List<Booking> bookings = new();
            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                using var cmd = new SqlCommand(
                    "SELECT TOUR_ID, TOUR_NAME, PLACE, Email, FirstName FROM booking WHERE Email=@Email", conn);
                cmd.Parameters.AddWithValue("@Email", userEmail);

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

        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login");

            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                using var cmd = new SqlCommand(
                    "DELETE FROM booking WHERE TOUR_ID=@TourID AND Email=@Email", conn);
                cmd.Parameters.AddWithValue("@TourID", id);
                cmd.Parameters.AddWithValue("@Email", userEmail);
                cmd.ExecuteNonQuery();
            }

            TempData["Message"] = "Booking deleted successfully!";
            return RedirectToAction("MyBooking");
        }

        [HttpGet]
        public IActionResult Order(string tourName = "", string place = "")
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login");

            var model = new Booking
            {
                TOUR_NAME = tourName,
                PLACE = place,
                Email = userEmail,
                FirstName = HttpContext.Session.GetString("UserName") ?? ""
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Order(Booking model)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login");

            using (var conn = new SqlConnection(_cs))
            {
                conn.Open();
                var sql = @"INSERT INTO booking (TOUR_NAME, PLACE, Email, FirstName) 
                    VALUES (@TOUR_NAME, @PLACE, @Email, @FirstName)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TOUR_NAME", model.TOUR_NAME);
                cmd.Parameters.AddWithValue("@PLACE", model.PLACE);
                cmd.Parameters.AddWithValue("@Email", userEmail);
                cmd.Parameters.AddWithValue("@FirstName", model.FirstName ?? "");

                cmd.ExecuteNonQuery();
            }

            TempData["Message"] = "Booking Successful!";
            return RedirectToAction("MyBooking");
        }

    }
}