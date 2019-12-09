using GasWeb.Shared.Users;

namespace GasWeb.Domain.Users.Entities
{
    internal class User
    {
        public long Id { get; private set; }
        public string NameId { get; private set; }
        public AuthenticationSchema AuthenticationSchema { get; private set; }
        public string Name { get; private set; }
        public UserRole Role { get; private set; }
        public bool Active { get; private set; }

        public User(string nameId, AuthenticationSchema authenticationSchema, string name, UserRole role, bool active)
        {
            NameId = nameId;
            AuthenticationSchema = authenticationSchema;
            Name = name;
            Role = role;
            Active = active;
        }

        public User(long id, string nameId, AuthenticationSchema authenticationSchema, string name, UserRole role, bool active)
        {
            Id = id;
            NameId = nameId;
            Name = name;
            Role = role;
            Active = active;
        }

        public void Update(UserUpdateModel updateModel)
        {
            Name = updateModel.Username;
            Role = updateModel.Role;
            Active = updateModel.Active;
        }
    }
}
