namespace MonolithModularNET.Extensions.Shared.Permissions;

public readonly struct UserPermissions
{
    public const string ViewAll = "user:view_all";
    public const string Create = "user:create";
    public const string Update = "user:update";
    public const string Delete = "user:delete";
    public const string SetRoles = "user:set_roles";
}