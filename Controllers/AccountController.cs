using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Tour_Management.Models;

namespace Tour_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _cs;

        public AccountController(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isValid = false;
            using (var conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                const string sql = "SELECT COUNT(1) FROM UserInfo WHERE email = @Email AND password = @Password";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = model.Email;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 256).Value = model.Password;

                var result = (int)await cmd.ExecuteScalarAsync();
                isValid = result > 0;
            }

            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetString("UserEmail", model.Email);
            return RedirectToAction("Index", "MainProfile");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                const string checkSql = "SELECT COUNT(1) FROM UserInfo WHERE email = @Email";
                using (var checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = model.Email;
                    var exists = (int)await checkCmd.ExecuteScalarAsync();
                    if (exists > 0)
                    {
                        ModelState.AddModelError(nameof(model.Email), "Email already registered.");
                        return View(model);
                    }
                }

                const string insertSql = @"
                    INSERT INTO UserInfo
                        (Email, FirstName, LastName, Gender, Password, dob, Street, City, State)
                    VALUES
                        (@Email, @FirstName, @LastName, @Gender, @Password, @dob, @Street, @City, @State)";
                using (var cmd = new SqlCommand(insertSql, conn))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = model.Email;
                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = model.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 100).Value = model.LastName;
                    cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 20).Value = model.Gender;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 256).Value = model.Password;
                    cmd.Parameters.Add("@dob", SqlDbType.Date).Value = model.Dob.Date;
                    cmd.Parameters.Add("@Street", SqlDbType.NVarChar, 200).Value = model.Street;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value = model.City;
                    cmd.Parameters.Add("@State", SqlDbType.NVarChar, 100).Value = model.State;

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

    }
}
