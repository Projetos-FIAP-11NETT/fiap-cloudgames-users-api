using FiapCloudGames.Users.Domain.Entities;

namespace FiapCloudGames.Users.Tests.Factories;

public class RoleFactory
{
    public static Role CreateAdminRole()
    {
        var role = new Role(1, "Admin");
        return role;
    }

    public static Role CreateUserRole()
    {
        var role = new Role(2, "User");
        return role;
    }
}