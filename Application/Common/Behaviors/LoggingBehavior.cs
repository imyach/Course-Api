using Application.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ICurrentUserService currentUserService) 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var userId = currentUserService.UserId;

            Log.Information("Client Request : {Name} {@UserId} {@Reuqest}", requestName, userId, request);
            var responce = await next();

            return responce;
        }
    }

}
