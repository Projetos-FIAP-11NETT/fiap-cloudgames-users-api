using FiapCloudGames.Users.Domain.Entities;

namespace FiapCloudGames.Users.Tests.Factories;

public static class UserFactory
{
    public static User CreateValidUser()
    {
        var user = new User("Hugo", "hugo1@gmail.com", "12345678q!", RoleFactory.CreateUserRole());
        return user;
    }
}

