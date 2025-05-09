using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Validation.DTO;

namespace Data.Validation
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Text).MaximumLength(300);
        }
    }

    public class CommentCreateDTOValidator : AbstractValidator<CommentCreateDTO>
    {
        public CommentCreateDTOValidator()
        {
            RuleFor(x => x.Text).MaximumLength(300);
        }
    }
}
