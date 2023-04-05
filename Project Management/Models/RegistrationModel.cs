using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Project_Management.Models
{
    public class RegistrationModel
    {

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [Remote(action: "IsEmailAvailable", controller: "Validation", ErrorMessage = "This email is already registered.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
