using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.CreateModule
{
    public class CreateModuleCommandValidator : AbstractValidator<CreateModuleCommand>
    {
        public CreateModuleCommandValidator() 
        {
            RuleFor(createModuleCommand => createModuleCommand.CourseId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(createModuleCommand => createModuleCommand.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(createModuleCommand => createModuleCommand.Title)
                .NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
