using Azure;
using MajlesMefa.Back.Repositories.Reddis;

namespace MajlesMefa.UI.Middleware
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRedisRepository redisRepo)
        {
            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
            // اگر مسیر لاگین باشد، از middleware عبور می‌کنیم
            if (context.Request.Path.StartsWithSegments("/Login"))
            {
                await _next(context);
                return;
            }

            var sessionToken = context.Request.Cookies["AspSessionToken"];
            var accessToken = context.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(sessionToken))
            {
                // بررسی blacklist بودن توکن
                if (await redisRepo.IsTokenBlacklistedAsync(sessionToken))
                {
                    await LogoutUser(context, redisRepo);
                    return;
                }
            }

            await _next(context);
        }

        private async Task LogoutUser(HttpContext context, IRedisRepository redisRepo)
        {
            
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = true
            };

            context.Response.Cookies.Append("AccessToken", "", cookieOptions);
            context.Response.Cookies.Append("RefreshToken", "", cookieOptions);
            context.Response.Cookies.Append("AspSessionToken", "", cookieOptions);
            context.Response.Cookies.Append(".AspNetCore.Session", "", cookieOptions);
            context.Response.Cookies.Append("_gui", "", cookieOptions);

            context.Response.Cookies.Delete("AccessToken");
            context.Response.Cookies.Delete("RefreshToken");
            context.Response.Cookies.Delete("AspSessionToken");
            context.Response.Cookies.Delete(".AspNetCore.Session");
            context.Response.Cookies.Delete("_gui");

            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";

            context.Response.Redirect("/Login");

            await Task.CompletedTask;
        }
    }
}
