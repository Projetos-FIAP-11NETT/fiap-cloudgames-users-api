using FiapCloudGames.Users.Application.UserFeature.Commands.AuthUser;
using FluentValidation.TestHelper;

namespace FiapCloudGames.Users.Tests.Application;

public class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator;

    public LoginUserCommandValidatorTests()
    {
        _validator = new LoginUserCommandValidator();
    }

    [Fact]
    public void ShouldHaveErroWhenPasswordIsEmpty()
    {
        // Arrange
        var command = new LoginUserCommand(
            Email: "joao@email.com",
            Password: ""
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("A senha não pode ser vazia.");
    }

    [Fact]
    public void ShouldHaveErroWhenEmailIsEmpty()
    {
        // Arrange
        var command = new LoginUserCommand(
            Email: "",
            Password: "abc1234*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("O e-mail não pode ser vazio.");
    }

    [Fact]
    public void ShouldHaveErroWhenEmailIsNotValid()
    {
        // Arrange
        var command = new LoginUserCommand(
            Email: "joao",
            Password: "abc1234*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("O e-mail deve conter um formato válido.");
    }

}