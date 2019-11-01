using GasWeb.Shared.Users;

namespace GasWeb.Shared.Authentication
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public User User { get; set; }
        public string ExternalUsername { get; set; }
    }
}
