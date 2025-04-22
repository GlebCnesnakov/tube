using FluentValidation;
using Data.Models;

namespace Data.Validation;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(x => x.Text).NotEmpty().WithMessage("Text cannot be empty").MinimumLength(10).WithMessage("Text must be more than 10 characters");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty").MinimumLength(5).WithMessage("Title must be more than 5 characters");
    }
}