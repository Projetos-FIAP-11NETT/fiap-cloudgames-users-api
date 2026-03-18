using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Domain.Exceptions;
using FiapCloudGames.Users.Tests.Factories;

namespace FiapCloudGames.Users.Tests.Domain;

public class UserTest
{
    #region validations for constructor

    [Theory]
    [InlineData("Douglas", "teste1@gmail.com", "Abcdef1!")]
    public void CreateUser_MustSucceed_WhenAllPropertiesIsValid(string name, string email, string password)
    {
        var role = RoleFactory.CreateUserRole();
        var user = new User(name, email, password, role);

        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.Contains(role, user.Roles);
    }

    #endregion validations for constructor

    #region validations for name property

    [Fact]
    public void PasswordProperty_MustThrowException_WhenNameIsEmpty()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("", "test1@gmail.com", "Abcdef1!", RoleFactory.CreateUserRole()));

        Assert.Equal("Nome inválido. Não pode ser vazio", exception.Message);
    }

    [Fact]
    public void PasswordProperty_MustThrowException_WhenNameIsTooLong()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User(new string('a', 51), "test1@gmail.com", "Abcdef1!", RoleFactory.CreateUserRole()));

        Assert.Equal("Nome inválido. Deve ter no máximo 50 caracteres.", exception.Message);
    }


    #endregion validations for name property

    #region validations for email property

    [Fact]
    public void EmailProperty_MustThrowException_WhenEmailIsEmpty()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", "", "Abcdef1!", RoleFactory.CreateUserRole()));

        Assert.Equal("E-mail inválido. Não pode ser vazio.", exception.Message);
    }

    [Fact]
    public void EmailProperty_MustThrowException_WhenEmailIsTooLong()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", new string('a', 256), "Abcdef1!", RoleFactory.CreateUserRole()));

        Assert.Equal("E-mail inválido. Deve ter no máximo 255 caracteres.", exception.Message);
    }

    [Theory]
    [InlineData("joaoemail.com")]
    [InlineData("joao@")]
    [InlineData("@email.com")]
    [InlineData("joao@email")]
    [InlineData("joao@ email.com")]
    public void EmailProperty_MustThrowException_WhenEmailFormatIsInvalid(string email)
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", email, "Abcdef1!", RoleFactory.CreateUserRole()));

        Assert.Equal("E-mail inválido. Formato incorreto.", exception.Message);
    }

    #endregion validations for email property

    #region validations for password property

    [Fact]
    public void PasswordProperty_MustThrowException_WhenPasswordIsEmpty()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", "test1@gmail.com", "", RoleFactory.CreateUserRole()));

        Assert.Equal("Senha inválida. Não pode ser vazia.", exception.Message);
    }

    [Fact]
    public void PasswordProperty_MustThrowException_WhenPasswordIsTooShort()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", "test1@gmail.com", new string('a', 7), RoleFactory.CreateUserRole()));

        Assert.Equal("Senha inválida. Deve ter no mínimo 8 caracteres.", exception.Message);
    }

    [Fact]
    public void PasswordProperty_MustThrowException_WhenPasswordHasNoLetter()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", "test1@gmail.com", "12345678!", RoleFactory.CreateUserRole()));

        Assert.Equal("Senha inválida. Deve conter pelo menos uma letra.", exception.Message);
    }

    [Fact]
    public void PasswordProperty_MustThrowException_WhenPasswordHasNoDigit()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", "test1@gmail.com", "Abcdefg!", RoleFactory.CreateUserRole()));

        Assert.Equal("Senha inválida. Deve conter pelo menos um número.", exception.Message);
    }

    [Fact]
    public void PasswordProperty_MustThrowException_WhenPasswordHasNoSpecialChar()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new User("Douglas", "test1@gmail.com", "Abcdefg1", RoleFactory.CreateUserRole()));

        Assert.Equal("Senha inválida. Deve conter pelo menos um caractere especial.", exception.Message);
    }

    #endregion validations for password property

    #region validations for setfirebaseuserid method

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task SetFirebaseUserId_MustThrowException_WhenValueIsInvalid(string firebaseUserId)
    {
        var user = UserFactory.CreateValidUser();

        var exception = await Assert.ThrowsAsync<DomainException>(() => 
            Task.Run(() => user.SetFirebaseUserId(firebaseUserId)));

        Assert.NotNull(exception);
        Assert.IsType<DomainException>(exception);
        Assert.Equal("FirebaseUserId inválido. Não pode ser vazio.", exception.Message);
    }

    [Theory]
    [InlineData("uuid-firebase-19175")]
    public async Task SetFirebaseUserId_MustSucced_WhenValueIsValid(string firebaseUserId)
    {
        var user = UserFactory.CreateValidUser();

        user.SetFirebaseUserId(firebaseUserId);

        Assert.Equal(firebaseUserId, user.FirebaseUserId);
    }

    #endregion validations for setfirebaseuserid method

    #region validations for addrole method

    [Fact]
    public void AddRole_MustSucced_WhenRoleIsValid()
    {
        var user = UserFactory.CreateValidUser();
        var role = RoleFactory.CreateUserRole();

        user.AddRole(role);

        Assert.Contains(role, user.Roles);
    }

    [Fact]
    public async Task AddRole_MustThrowException_WhenRoleIsInvalid()
    {
        var user = UserFactory.CreateValidUser();

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            Task.Run(() => user.AddRole(null)));

        Assert.NotNull(exception);
        Assert.IsType<DomainException>(exception);
        Assert.Equal("Perfil inválido. Não pode ser vazio.", exception.Message);
    }

    #endregion validations for addrole method

    #region validations for makeadmin method

    [Fact]
    public void MakeAdmin_MustSucced_WhenClearAllRolesAndSetAdmin()
    {
        var user = UserFactory.CreateValidUser();
        var role = RoleFactory.CreateAdminRole();

        user.MakeAdmin(role);

        Assert.Single(user.Roles);
        Assert.Contains(role, user.Roles);
    }

    #endregion validations for makeadmin method
}