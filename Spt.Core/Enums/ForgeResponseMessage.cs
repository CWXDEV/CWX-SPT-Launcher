namespace Spt.Core.Enums;

public record ForgeResponseMessage()
{
    // TODO: these need to be more consistent on the API
    public const string InvalidCredentials = "invalid credentials";
    public const string InvalidEmail = "The email field must be a valid email address.";
    public const string Success = "success";
    public const string Authenticated = "authenticated";
    public const string Unauthenticated = "Unauthenticated.";
}
