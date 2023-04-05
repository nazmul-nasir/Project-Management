using Microsoft.AspNetCore.Identity;

namespace Project_Management.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
        public string? Token { get; set; }
    }
}
