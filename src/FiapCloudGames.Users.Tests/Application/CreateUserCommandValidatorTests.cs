using FiapCloudGames.Users.Application.UserFeature.Commands.CreateUser;
using FluentValidation.TestHelper;

namespace FiapCloudGames.Users.Tests.Application;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: "",
            email: "joao@email.com",
            password: "Abc12345*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("O nome não pode ser vazio.");
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsTooLong()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: new string('a', 51),
            email: "joao@email.com",
            password: "Abc12345*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("O nome deve conter no máximo 50 caracteres.");
    }

    [Fact]
    public void ShouldHaveErrorWhenEmailIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: "João Silva",
            email: "",
            password: "Abc12345*"
        );

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
        var command = new CreateUserCommand(
            name: "João Silva",
            email: "joao",
            password: "Abc12345*"
        );

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
        var command = new CreateUserCommand(
            name: "João Silva",
            email: new string('a', 256),
            password: "Abc12345*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("O e-mail deve conter no máximo 255 caracteres.");
    }

    [Fact]
    public void ShouldHaveErrorWhenPasswordIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: "João Silva",
            email: "joao@email.com",
            password: ""
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("A senha não pode ser vazia.");
    }

    [Fact]
    public void ShouldHaveErrorWhenPasswordHasLessThen8Charateres()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: "João Silva",
            email: "joao@email.com",
            password: "Abc123*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("A senha deve conter ao menos 8 caracteres.");
    }

    [Fact]
    public void ShouldHaveErrorWhenPasswordHasNoLetter()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: "João Silva",
            email: "joao@email.com",
            password: "1234567*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("A senha deve conter ao menos uma letra.");
    }

    [Fact]
    public void ShouldHaveErrorWhenPasswordHasNoNumber()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: "João Silva",
            email: "joao@email.com",
            password: "abcdfgh*"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("A senha deve conter ao menos um número.");
    }

    [Fact]
    public void ShouldHaveErrorWhenPasswordHasNoSpecialCharacter()
    {
        // Arrange
        var command = new CreateUserCommand(
            name: "João Silva",
            email: "joao@email.com",
            password: "Abc12345"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("A senha deve conter ao menos um caractere especial.");
    }
}
