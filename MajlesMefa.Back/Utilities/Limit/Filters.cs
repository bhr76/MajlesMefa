using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Net;

namespace MajlesMefa.Back.Utilities.Limit
{

    [AttributeUsage(AttributeTargets.Method)]
    public class RequestLimitAttribute : ActionFilterAttribute
    {

        public int NoOfRequest { get; set; } = 1;
        public int Seconds { get; set; } = 1;
        private static MemoryCache Cache { get; } = new(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = ((ControllerBase)context.Controller)
                                .ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((ControllerBase)context.Controller)
                                .ControllerContext.ActionDescriptor.ActionName;

            var userService = (ICurrentUserService)context.HttpContext.RequestServices.GetService(typeof(ICurrentUserService));

            var memoryCacheKey = $"{controllerName}_{actionName}_{((!string.IsNullOrEmpty(userService?.GetCurrentUser()?.UserId.ToString())) ? userService.GetCurrentUser().UserId : userService.GetCurrentUser().IpAddress)}";
            Cache.TryGetValue(memoryCacheKey, out int prevReqCount);
            if (prevReqCount >= NoOfRequest)
            {
                context.Result = new ContentResult
                {
                    //Content = $"<h1>Request limit is exceeded. Try again in {Seconds} seconds.",
                    Content = "<div style='background: url(../../images/error_down.png) no-repeat; height: 100%; background-position : center; background-size: contain' class='h-100 d-flex align-items-center justify-content-center error-rate-limit'>" +
                    
                    "</div>",
                    ContentType= "text/html; charset=utf-8",
                };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(Seconds));
                Cache.Set(memoryCacheKey, prevReqCount + 1, cacheEntryOptions);
            }

        }
       
      

    }


}


