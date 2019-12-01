namespace GasWeb.Shared.Users
{
    public class UserUpdateModel
    {
        public string Username { get; set; }
        public UserRole? Role { get; set; }
        public bool? Active { get; set; }
    }
}
