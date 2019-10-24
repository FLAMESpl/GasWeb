using GasWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;

namespace GasWeb.Server
{
    internal class RequireAdminRoleAttribute : AuthorizeAttribute
    {
        public RequireAdminRoleAttribute()
        {
            Roles = UserRole.Admin.ToString();
        }
    }

    internal class RequireModeratorRoleAttribute : AuthorizeAttribute
    {
        public RequireModeratorRoleAttribute()
        {
            Roles = $"{UserRole.Admin},{UserRole.Moderator}";
        }
    }

    internal class RequireUserRoleAttribute : AuthorizeAttribute
    {
    }
}
