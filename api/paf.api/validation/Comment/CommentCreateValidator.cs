using FluentValidation;
using paf.api.Dtos.Comment_Dtos;

namespace paf.api.validation.Comment
{
    public class CommentUpdateValidator:AbstractValidator<CommentCreateDto>
    {
        public CommentUpdateValidator()
        {
            RuleFor(c=>c.TheComment).NotEmpty().MinimumLength(1);

        }
    }
}
