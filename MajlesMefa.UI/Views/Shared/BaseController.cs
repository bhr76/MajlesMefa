using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.UseCases.Queries.GetUserMenuQuery;

namespace MajlesMefa.UI.Views.Shared
{
    public class BaseController : Controller
    {
        public const string CONTROLLER = "Base";
        protected readonly IMapper _mapper;


        public BaseController(IMapper mapper)
        {
            _mapper = mapper;   
        }
       
        private IMediator _mediator;

        public IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public IMapper Mapper { get; }

        public IActionResult Signout()
        {
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Delete("AspSessionToken");

            return RedirectToAction("Index", "Login");
        }


    }

}