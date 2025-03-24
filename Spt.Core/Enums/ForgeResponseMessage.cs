namespace Spt.Core.Enums;

public record ForgeResponseMessage()
{
    // TODO: these need to be more consistent on the API
    public const string InvalidCredentials = "invalid credentials";
    public const string Success = "success";
    public const string Unauthenticated = "Unauthenticated.";
}
