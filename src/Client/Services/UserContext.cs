using GasWeb.Shared.Users;

namespace GasWeb.Client.Services
{
    public class UserContext
    {
        public UserContext() { }

        public UserContext(long id, string name, UserRole role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }
}
