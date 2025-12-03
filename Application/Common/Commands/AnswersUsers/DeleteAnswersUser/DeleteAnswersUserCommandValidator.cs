using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.AnswersUsers.DeleteAnswersUser
{
    public class DeleteAnswersUserCommandValidator : AbstractValidator<DeleteAnswersUserCommand>
    {
        public DeleteAnswersUserCommandValidator()
        {
            RuleFor(deleteAnswersUserCommand => deleteAnswersUserCommand.Id)
                .NotEqual(Guid.Empty).NotNull();
        }
    }
}
