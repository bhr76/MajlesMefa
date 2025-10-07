using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Queries.GetDashboardQuery;
using MajlesMefa.Back.UseCases.Queries.GetUserMenuQuery;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.Home
{
    [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
    public class SupportController : BaseController
    {
        private readonly ILogger<SupportController> _logger;

        public SupportController(IMapper mapper, ILogger<SupportController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [Display(Name = "مدیریت پشتیبانی")]
        [Auth]
        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult Index()
        {
            return View();
        }
    }
}