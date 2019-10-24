using GasWeb.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GasWeb.Server.Validation
{
    public class ValidationExceptionTranslatorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is EntityNotFound)
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
