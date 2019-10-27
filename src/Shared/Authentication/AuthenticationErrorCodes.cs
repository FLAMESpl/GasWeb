namespace GasWeb.Shared.Authentication
{
    public static class AuthenticationErrorCodes
    {
        public const string NotAuthenticatedByExternalProvider = nameof(NotAuthenticatedByExternalProvider);
        public const string NotRegistered = nameof(NotRegistered);
        public const string AlreadyRegistered = nameof(AlreadyRegistered);
    }
}
