using System.ComponentModel.DataAnnotations;

namespace Tour_Management.Models
{
    public class AddTourViewModel
    {
        [Required]
        [Display(Name = "Tour Name")]
        public string TourName { get; set; }

        [Required]
        public string Place { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        public string Locations { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Characters must be less than 250.")]
        public string TourInfo { get; set; }

        [Required]
        [Display(Name = "Tour Image")]
        public IFormFile Pic { get; set; }
    }
}