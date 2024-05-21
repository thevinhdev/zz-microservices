using FluentValidation;

namespace IOIT.Identity.Application.Users.Commands.Create
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email not empty");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName not empty");
        }
    }
}
