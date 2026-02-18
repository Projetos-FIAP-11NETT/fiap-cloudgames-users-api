using FiapCloudGames.Users.Application.UserFeature.Commands.MakeAdmin;
using FluentValidation.TestHelper;

namespace FiapCloudGames.Users.Tests.Application;

public class MakeAdminCommandValidatorTest
{
    private readonly MakeAdminCommandValidator _validator = new();

    [Fact]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        // Arrange
        var command = new MakeAdminCommand("");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("O e-mail não pode ser vazio.");
    }

    [Fact]
    public void ShouldHaveErrorWhenEmailIsNotValid()
    {
        // Arrange
        var command = new MakeAdminCommand("joao");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("O e-mail deve conter um formato válido.");
    }

    [Fact]
    public void ShouldHaveErrorWhenEmailIsTooLong()
    {
        // Arrange
        var command = new MakeAdminCommand(new string('a', 256));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("O e-mail deve conter no máximo 255 caracteres.");
    }
}