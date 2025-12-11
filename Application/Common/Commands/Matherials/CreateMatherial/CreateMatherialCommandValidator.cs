using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.CreateMatherial
{
    public class CreateMatherialCommandValidator : AbstractValidator<CreateMatherialCommand>
    {
        public CreateMatherialCommandValidator() 
        {
            RuleFor(createMatherialCommand => createMatherialCommand.ModuleId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createMatherialCommand => createMatherialCommand.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(createMatherialCommand => createMatherialCommand.Title)
                .NotEmpty().NotNull().MaximumLength(100);
            RuleFor(createMatherialCommand => createMatherialCommand.Order)
                .NotEmpty().NotNull();
            RuleFor(createMatherialCommand => createMatherialCommand.Description)
                .MaximumLength(1000);
        }
    }
}
