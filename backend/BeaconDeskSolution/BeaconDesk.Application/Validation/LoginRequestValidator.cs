using BeaconDesk.Application.Dto.AuthenticacionDto;
using FluentValidation;

namespace BeaconDesk.Application.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            //Regla de Validacion de Email

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El campo Email es obligatorio.")
                .EmailAddress().WithMessage("El campo Email debe ser una dirección de correo electrónico válida.");

            //Regla de Validacion de Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("El campo Password es obligatorio.")
                .MinimumLength(6).WithMessage("El campo Password debe tener al menos 6 caracteres.");
        }

    }
}
