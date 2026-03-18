using FluentValidation;

namespace FiapCloudGames.Users.Application.UserFeature.Commands.MakeAdmin;

public sealed class MakeAdminCommandValidator : AbstractValidator<MakeAdminCommand>
{
    public MakeAdminCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail não pode ser vazio.")
            .MaximumLength(255).WithMessage("O e-mail deve conter no máximo 255 caracteres.")
            .EmailAddress().WithMessage("O e-mail deve conter um formato válido.");
    }
}