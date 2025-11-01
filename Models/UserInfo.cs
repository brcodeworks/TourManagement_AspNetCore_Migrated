using System;
using System.ComponentModel.DataAnnotations;

namespace Tour_Management.Models
{
    public class UserInfo
    {
        [Key]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Gender { get; set; }

        [Required]
        public string Password { get; set; }

        public string City { get; set; }
    }
}
