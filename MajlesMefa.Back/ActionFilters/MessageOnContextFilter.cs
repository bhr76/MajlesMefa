using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.ActionFilters
{
    public class MessageOnContextAttribute : TypeFilterAttribute
    {
        public MessageOnContextAttribute(string viewPath) : base(typeof(MessageOnContextFilter))
        {
            Arguments = new object[] { viewPath };
        }
    }

    public class MessageOnContextFilter : IActionFilter
    {
        private readonly string _viewPath;

        public MessageOnContextFilter(string viewPath)
        {
            _viewPath = viewPath;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.HttpContext.Items.Any(x => x.Key.ToString() == "displayOnItems"))
            {
                context.HttpContext.Items["displayOnItems"] = "true";
            }
            else
            {
                context.HttpContext.Items.Add("displayOnItems", "true");
            }

            if (context.HttpContext.Items.Any(x => x.Key.ToString() == "viewPath"))
            {
                context.HttpContext.Items["viewPath"] = _viewPath;
            }
            else
            {
                context.HttpContext.Items.Add("viewPath", _viewPath);
            }
        }
    }
}
