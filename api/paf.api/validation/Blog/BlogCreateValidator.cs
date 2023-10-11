using FluentValidation;
using paf.api.Dtos.Blog_Dtos;

namespace paf.api.validation.Blog
{
    public class BlogCreateValidator:AbstractValidator<BlogCreateDto>
    {
        public BlogCreateValidator()
        {
            RuleFor(b=>b.Heading).NotEmpty().MinimumLength(3);
            RuleFor(b => b.Content).NotEmpty().MinimumLength(3);


        }
    }
}
