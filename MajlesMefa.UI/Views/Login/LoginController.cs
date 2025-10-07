using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.UseCases.Commmands.LoginByRTokenCommand;
using MajlesMefa.Back.UseCases.Commmands.LoginCommand;
using MajlesMefa.Back.Utilities.Captcha;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Models.Auth;
using MajlesMefa.UI.Views.Home;
using MajlesMefa.UI.Views.Shared;
using System.Diagnostics;
using System.Web;
using MajlesMefa.Back.Repositories.Reddis;

namespace MajlesMefa.UI.Core.Login
{
    public class LoginController : BaseController
    {
        public const string CONTROLLER = "Login";
        private readonly ILogger<LoginController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginController(ILogger<LoginController> logger,
                         IMapper mapper,
                         IHttpContextAccessor contextAccessor) : base(mapper)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> LoginByRefreshToken(string redirectUrl)
         {
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["RefreshToken"]))
            {
                return RedirectToAction("index");
            }
            else
            {
                var token = await Mediator.Send(new LoginByRTokenCommand()
                {
                    RefreshToken = HttpContext.Request.Cookies["RefreshToken"],
                });

                HttpContext.Response.Cookies.Append("AccessToken", token.AccessToken, new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddSeconds(token.AccessTokenExpiresInSeconds),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure= true,
                });
                //HttpContext.Response.Cookies.Append("RefreshToken", token.RefreshToken, new CookieOptions()
                //{
                    //Expires = DateTimeOffset.Now.AddSeconds(token.RefresshTokenExpiresInSeconds),
                //    HttpOnly = true,
                //    SameSite = SameSiteMode.Strict
                //});

                return Redirect(HttpUtility.UrlDecode(redirectUrl));
            }

             
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        [MessageOnContext("~/Views/Login/Index.cshtml")]
        public async Task<IActionResult> Index(LoginVm loginVm)
        {
            var isValid = Captcha.ValidateCaptchaCode(loginVm.CaptchaCode, HttpContext);

            if (isValid)
            {
                var userId = loginVm.Username;

                var existingSession = await RedisRepository.GetUserSessionAsync(userId);
                if (!string.IsNullOrEmpty(existingSession))
                {
                    await RedisRepository.AddToBlacklistAsync(existingSession, TimeSpan.FromHours(1));
                }

                var token = await Mediator.Send(new LoginCommand()
                {
                    Username = loginVm.Username,
                    Password = loginVm.Password,
                });

                // ذخیره نشست جدید در Redis
                await RedisRepository.SetUserSessionAsync(
                    userId,
                    token.SessionToken,
                    TimeSpan.FromSeconds(token.RefresshTokenExpiresInSeconds));

                Response.Cookies.Append("AccessToken", token.AccessToken, new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddSeconds(token.AccessTokenExpiresInSeconds),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                });
                Response.Cookies.Append("RefreshToken", token.RefreshToken, new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddSeconds(token.RefresshTokenExpiresInSeconds),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                });
                Response.Cookies.Append("AspSessionToken", token.SessionToken, new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                });

                return Json(new { redirectToUrl = Url.Action("Index", "Loan") });
            }
            else
            {
                throw new Exception("اطلاعات ورود نامعتبر است");
            }
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            const int width = 280;
            const int height = 81;
            var code = Captcha.GenerateCaptchaCode();
            var (captchaCode, captchaByte) = Captcha.GenerateCaptchaImage(width, height, code);
            HttpContext.Session.SetString("CaptchaCode", captchaCode);

            Stream s = new MemoryStream(captchaByte);
            return new FileStreamResult(s, "image/png");
        }

    }
}