using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeApi.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class LogErrorAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<IAppLogger>();
            logger?.Log(LogType.Error, () => context.Exception.Message, context.Exception);
        }
    }
}
