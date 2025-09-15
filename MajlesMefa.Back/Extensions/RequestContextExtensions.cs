using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Extensions
{
    public static class RequestContextExtensions
    {
        public static bool IsDiplayMessageOnContext(this HttpContext context)
        {
            return context.Items.Keys.Any(x => x.ToString() == "displayOnItems") && context.Items["displayOnItems"].ToString() == "true";
        }

        public static void WriteMessageOnContext<TMessage>(this HttpContext context, TMessage error)
        {
            if(context.Items.Any(x => x.Key.ToString() == "message"))
            {
                context.Items.Add("message", error);
            }
            else
            {
                context.Items["message"] = error;
            }
        }

        public static TMessage ReadMessageOnContext<TMessage>(this HttpContext context)
        {
            if(context.Items.Any(x => x.Key.ToString() == "message"))
            {
                return (TMessage)context.Items["message"];
            }
            else
            {
                return default;
            }
        }


    }
}
