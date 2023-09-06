using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace MS.IConstruye
{
    public class RequestExceptionFilter<T> : IExceptionFilter where T : class
    {
        private readonly ILogger<RequestExceptionFilter<T>> logger;

        public RequestExceptionFilter(ILogger<RequestExceptionFilter<T>> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.GetType() == typeof(T))
            {
                context.Result = new BadRequestObjectResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                context.Result = new ObjectResult(context.Exception.Message) { StatusCode = StatusCodes.Status500InternalServerError };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }
    }
}
