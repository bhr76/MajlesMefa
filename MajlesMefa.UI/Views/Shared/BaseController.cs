using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.UseCases.Queries.GetUserMenuQuery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MajlesMefa.Back.Repositories.Reddis;
using System.IdentityModel.Tokens.Jwt;
using DocumentFormat.OpenXml.InkML;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Dtos.UserDtos;

namespace MajlesMefa.UI.Views.Shared
{
    public class BaseController : Controller
    {
        public const string CONTROLLER = "Base";
        protected readonly IMapper _mapper;
        private IMediator _mediator;
        private IRedisRepository _redisRepository;

        protected IRedisRepository RedisRepository =>
        _redisRepository ??= HttpContext.RequestServices.GetService<IRedisRepository>();


        public IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public IMapper Mapper => _mapper;

        // فقط یک constructor با پارامترهای ضروری
        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        private IMediator _mediator;

        public IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public async Task<IActionResult> Signout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var sessionToken = Request.Cookies["AspSessionToken"];
            var userId = await GetCurrentUserId(); 

            if (!string.IsNullOrEmpty(sessionToken))
            {
                await RedisRepository.AddToBlacklistAsync(sessionToken, TimeSpan.FromHours(1));
               
                // حذف نشست کاربر از Redis
                await RedisRepository.RemoveUserSessionAsync(userId);
            }

            // پاک کردن کوکی‌ها
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = true
            };

            Response.Cookies.Append("AccessToken", "", cookieOptions);
            Response.Cookies.Append("RefreshToken", "", cookieOptions);
            Response.Cookies.Append("AspSessionToken", "", cookieOptions);
            Response.Cookies.Append(".AspNetCore.Session", "", cookieOptions);
            Response.Cookies.Append("_gui", "", cookieOptions);

            // حذف کوکی‌ها
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Delete("AspSessionToken");
            Response.Cookies.Delete(".AspNetCore.Session");
            Response.Cookies.Delete("_gui");

            await HttpContext.Session.LoadAsync();
            HttpContext.Session.Clear();
            await HttpContext.Session.CommitAsync();

            await HttpContext.SignOutAsync();

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Index", "Login");
        }

        private async Task<string> GetCurrentUserId()
        {
            try
            {
                var accessToken = Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(accessToken))
                    return null;

                // اگر توکن شما JWT است، می‌توانید آن را decode کنید
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(accessToken))
                {
                    var token = handler.ReadJwtToken(accessToken);
                    var userId = token.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
                    return userId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get user ID from token");
            }

            return null;
        }


    }

}