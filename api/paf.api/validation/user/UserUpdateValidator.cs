using FluentValidation;
using paf.api.Dtos.User_Dtos;

namespace paf.api.validation.user
{
    public class UserUpdateValidator:AbstractValidator<UserUpdateDto>
    {

        public UserUpdateValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Age).NotEmpty().GreaterThan(15);
            RuleFor(u => u.Phone).NotEmpty().MaximumLength(13);
            RuleFor(u => u.PostalCode).NotEmpty().MaximumLength(10);
            RuleFor(u => u.Gender).NotEmpty();
            RuleFor(u => u.UserName).NotEmpty().MaximumLength(10).MinimumLength(1);


        }
    }
}
