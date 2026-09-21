using System.Diagnostics;
using EmployeeApi.Logging;

namespace EmployeeApi.Middleware
{
    public class HeaderLogMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAppLogger logger)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;
                var logType = statusCode >= StatusCodes.Status500InternalServerError
                    ? LogType.Error
                    : LogType.Info;

                logger.Log(
                    logType,
                    () => $"HTTP {context.Request.Method} {context.Request.Path} responded {statusCode} in {stopwatch.Elapsed.TotalMilliseconds:0.0000} ms",
                    new
                    {
                        context.Request.Method,
                        Path = context.Request.Path.ToString(),
                        QueryString = context.Request.QueryString.ToString(),
                        StatusCode = statusCode,
                        ElapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds
                    });
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                logger.Log(
                    LogType.Error,
                    () => $"HTTP {context.Request.Method} {context.Request.Path} failed in {stopwatch.Elapsed.TotalMilliseconds:0.0000} ms",
                    exception);

                throw;
            }
        }
    }
}
