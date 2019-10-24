using GasWeb.Shared.Users;
using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.Authentication
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
