﻿namespace GasWeb.Shared.Users
{
    public class User
    {
        public User() { }

        public User(long id, string name, UserRole role, bool active)
        {
            Id = id;
            Name = name;
            Role = role;
            Active = active;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public bool Active { get; set; }
    }
}
