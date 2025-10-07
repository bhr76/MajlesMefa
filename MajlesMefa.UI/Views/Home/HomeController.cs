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
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMapper mapper): base(mapper)
        {
            _logger = logger;
        }

        [Display(Name = "داشبورد")]
        [Auth]
        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Index()
        {

            var dashboard = await Mediator.Send(new GetDashboardQuery());
            var mokatebe = await Mediator.Send(new GetDashboardMokatebeByResponseStatusQuery());
            var mokatebeStatus = await Mediator.Send(new GetDashboardMokatebeByStatusQuery());
            var mokatebePaynevesht = await Mediator.Send(new GetDashboardMokatebeByResponseStatusQuery(MokatebeTypeEnum.PeyNevesht));
            var mokatebePeyneveshtStatus = await Mediator.Send(new GetDashboardMokatebeByStatusQuery(MokatebeTypeEnum.PeyNevesht));
            var mokatebeByOrganization = await Mediator.Send(new GetDashboardMokatebeByOrganizationQuery());
            var molaghat = await Mediator.Send(new GetDashboardMolaghatByCityQuery());
            var molaghatByMonth = await Mediator.Send(new GetDashboardMolaghatByMonthQuery());
            var soalByCity = await Mediator.Send(new GetDashboardSoalatByCityQuery());
            var soalByOrganization = await Mediator.Send(new GetDashboardSoalatByOrganizationQuery());
            var soalByStatus = await Mediator.Send(new GetDashboardSoalatByStatusQuery());

            //mokatebe-by-answer
            ViewBag.dashboardMokatebeData = mokatebeStatus;
            ViewBag.dashboardDetailsMokatebeData = mokatebe;

            //mokatebe-peynevesht-by-answer
            ViewBag.dashboardMokatebePeyneveshtData = mokatebePeyneveshtStatus;
            ViewBag.dashboardDetailsMokatebePeyneveshtData = mokatebePaynevesht;

            //data
            ViewBag.dashboardData = dashboard.Select(x => x.ItemName).ToArray();
            ViewBag.dashboardCountData = dashboard.Select(x => x.Count).ToArray();

            //molaghat-data
            ViewBag.molaghatData = molaghat.Select(x => x.ItemName).ToArray();
            ViewBag.molaghatCountData = molaghat.Select(x => x.Count).ToArray();

            //molaghat-by-month
            ViewBag.molaghatByMonthData = molaghatByMonth.Select(x => x.ItemName).ToArray();
            ViewBag.molaghatByMonthCountData = molaghatByMonth.Select(x => x.Count).ToArray();

            //soal-by-city
            ViewBag.soalByCityData = soalByCity.Select(x => x.ItemName).ToArray();
            ViewBag.soalByCityCountData = soalByCity.Select(x => x.Count).ToArray();

            //soal-by-organization
            ViewBag.soalByOrganizationData = soalByOrganization.Select(x => x.ItemName).ToArray();
            ViewBag.soalByOrganizationCountData = soalByOrganization.Select(x => x.Count).ToArray();

            //soal-by-status
            ViewBag.soalByStatusData = soalByStatus.Select(x => x.ItemName).ToArray();
            ViewBag.soalByStatusCountData = soalByStatus.Select(x => x.Count).ToArray();

            //mokatebe-by-organization
            ViewBag.mokatebeByOrganizationData = mokatebeByOrganization?.Select(x => x.ItemName).ToArray();
            ViewBag.mokatebeByOrganizationCountData = mokatebeByOrganization?.Select(x => x.Count).ToArray();

            return View(dashboard);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult Privacy()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult NotFound1()
        {
            return View("NotFound");
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}