using GasWeb.Shared.Users;

namespace GasWeb.Domain.Users.Entities
{
    internal class User
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; private set; }
        public bool Active { get; private set; }

        public User(string name, string password, UserRole role, bool active)
        {
            Name = name;
            Password = password;
            Role = role;
            Active = active;
        }

        public void Update(UserUpdateModel updateModel)
        {
            Name = updateModel.Username ?? Name;
            Role = updateModel.Role ?? Role;
        }
    }
}
