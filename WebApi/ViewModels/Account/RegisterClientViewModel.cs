using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels.Account
{
    public class RegisterClientViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "First name can't be longer than 100 characters")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Last name can't be longer than 100 characters")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Gender can't be longer than 20 characters")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Birthday")]
        public DateTime BirthDay { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Street name can't be longer than 100 characters")]
        [Display(Name = "Street")]
        public string StreetName { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "House number can't be longer than 100 characters")]
        [Display(Name = "Housenumber")]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Province can't be longer than 100 characters")]
        [Display(Name = "Province")]
        public string Province { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "District can't be longer than 100 characters")]
        [Display(Name = "District")]
        public string District { get; set; }
    }
}