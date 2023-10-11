using FluentValidation;
using paf.api.Dtos.Blog_Dtos;

namespace paf.api.validation.Blog
{
    public class BlogUpdateValidator:AbstractValidator<BlogUpdateDto>
    {
        public BlogUpdateValidator()
        {
            RuleFor(b => b.Heading).NotEmpty().MinimumLength(3);
            RuleFor(b => b.Content).NotEmpty().MinimumLength(3);


        }
    }
}
