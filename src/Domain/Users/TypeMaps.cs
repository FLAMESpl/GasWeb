namespace GasWeb.Domain.Users
{
    internal static class TypeMaps
    {
        public static Shared.Users.User ToContract(this Entities.User user)
        {
            return new Shared.Users.User(user.Id, user.Name, user.Role, user.Active);
        }
    }
}
