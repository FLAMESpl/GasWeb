using System.ComponentModel.DataAnnotations;

namespace GasWeb.Shared.Authentication
{
    public class LoginModel
    {
        [Required]
        public string NameId { get; set; }
    }
}
