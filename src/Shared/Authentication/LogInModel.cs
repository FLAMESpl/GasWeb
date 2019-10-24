using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.Authentication
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
