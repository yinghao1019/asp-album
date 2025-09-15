using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using asp_album.Exceptions;
using asp_album.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace asp_album.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, ex, StatusCodes.Status401Unauthorized);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, int httpStatusCode)
        {
            // 取得 REQUEST 的唯一識別碼
            var traceId = context.TraceIdentifier;
            _logger.LogError(exception, "traceId={traceId} --> An unexpected error occurred.", traceId);

            await new JsonResult(new ErrorViewModel()
            {
                RequestId = traceId,
                StatusCode = httpStatusCode,
                Message = exception.Message
            })
            {
                StatusCode = httpStatusCode
            }.ExecuteResultAsync(new ActionContext { HttpContext = context });
        }
    }
}