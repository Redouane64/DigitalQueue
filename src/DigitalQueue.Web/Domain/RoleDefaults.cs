using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Domain;

public class RoleDefaults
{
    public static readonly IdentityRole Student = new IdentityRole("student");
    public static readonly IdentityRole Administrator = new IdentityRole("administrator");
    public static readonly IdentityRole Teacher = new IdentityRole("teacher");
}
