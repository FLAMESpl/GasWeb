namespace GasWeb.Shared.Users
{
    public class User
    {
        public User(long id, string name, UserRole role, bool active)
        {
            Id = id;
            Name = name;
            Role = role;
            Active = active;
        }

        public long Id { get; }
        public string Name { get; }
        public UserRole Role { get; }
        public bool Active { get; }
    }
}
