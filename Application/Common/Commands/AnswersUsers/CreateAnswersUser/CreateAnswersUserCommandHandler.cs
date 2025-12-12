using Application.Common.Commands.Matherials.CreateMatherial;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.AnswersUsers.CreateAnswersUser
{
    public class CreateAnswersUserCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateAnswersUserCommand, Guid>
    {
        public async Task<Guid> Handle(CreateAnswersUserCommand request, CancellationToken cancellationToken)
        {
            var answersUser = new AnswersUser
            {
                Id = Guid.NewGuid(),
                AnswerId = request.AnswerId,
                UserId = request.CurrentUserId,
            };

            await context.AnswersUsers.AddAsync(answersUser, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return answersUser.Id;
        }
    }
}
