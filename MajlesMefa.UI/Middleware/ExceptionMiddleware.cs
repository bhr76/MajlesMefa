using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using NToastNotify;
using MajlesMefa.Back.Extensions;
using MajlesMefa.UI.Models;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MajlesMefa.UI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //public INotyfService _notifyService { get; }
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            //_notifyService = notifyService; 
        }

        public async Task InvokeAsync(HttpContext httpContext, IConfiguration configuration, IToastNotification _notifyService)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, configuration, _notifyService);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, IConfiguration configuration, IToastNotification _notifyService)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ErrorDetails result;

            if (exception is UnauthorizedAccessException)
            {
                if (Regex.IsMatch(exception.Message, @"^.*([\u0600-\u06FF]).*$"))
                {

                    if (exception.Message.IndexOf(':') > -1)
                    {
                        int substringStartIndex = exception.Message.IndexOf(':') + 1;
                        int length = exception.Message.Length - substringStartIndex;
                        string newStr = exception.Message.Substring(substringStartIndex, length);
                        result = new ErrorDetails(
                            context.Response.StatusCode,
                            newStr,
                            exception.StackTrace ?? context.Request.Path,
                            null
                        );
                    }
                    else
                    {
                        result = new ErrorDetails(
                            context.Response.StatusCode,
                            exception.Message,
                            exception.StackTrace ?? context.Request.Path,
                            null
                        );
                    }


                }
                else
                {
                    result = new ErrorDetails(
                    context.Response.StatusCode,
                    "خطا در انجام عملیات!",
                        exception.StackTrace ?? context.Request.Path,
                        null
                    );
                }

            }
            else if (context.Response.StatusCode == 403)
            {
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    "شما مجوز دسترسی به این بخش را ندارید!",
                    exception.StackTrace ?? context.Request.Path,
                    null
                );
            }
            else if (context.Response.StatusCode == 204)
            {
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    "داده ای از سمت سرویس دریافت نشد!",
                    exception.StackTrace ?? context.Request.Path,
                    null
                );
            }
            else if (exception is DbUpdateException && exception.InnerException?.Message.Contains("DELETE statement conflicted") == true)
            {
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    "رکورد انتخابی دارای اطلاعات وابسته است!",
                    exception.StackTrace ?? context.Request.Path,
                    null
                );
            }
            else if (exception is AccessViolationException)
            {
                var r = Regex.IsMatch(exception.Message, @"^[\u0600-\u06FF]+$");
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    exception.Message,
                    exception.StackTrace ?? context.Request.Path,
                    null
                );
            }
            else if (exception is InvalidOperationException)
            {
                //var r = Regex.IsMatch(exception.Message, "^[آ-ی]$");
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    exception.Message,
                    exception.StackTrace ?? context.Request.Path,
                    null
                );
            }
            else if (exception is NullReferenceException)
            {
                result = new ErrorDetails(
                   context.Response.StatusCode,
                   exception.Message,
                    exception.StackTrace ?? context.Request.Path,
                    null
               );
            }
            else
            {
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    "سیستم با خطا مواجه شده است..!",
                    context.Request.Path,
                    null
                );
            }
            File.AppendAllText(@"c:\test\log.txt", $"{exception.Message} - {exception.StackTrace} - {DateTime.Now.ToString()}");
            //if (context.IsDiplayMessageOnContext())
            //{
            //    context.WriteMessageOnContext(result);
            //    return GetViewResultTask(context, context.Items["viewPath"].ToString());
            //}
            //else
            //{
            return context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
            //}
        }


        private Task GetViewResultTask(HttpContext context, string viewName)
        {
            var viewResult = new ViewResult()
            {
                ViewName = viewName
            };

            var executor = context.RequestServices.GetRequiredService<IActionResultExecutor<ViewResult>>();
            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData,
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            return executor.ExecuteAsync(actionContext, viewResult);
        }
    }
}
