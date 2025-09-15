using AutoMapper;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Enums.Soval;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;
using MajlesMefa.Back.UseCases.Queries.GetPagesQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.Page
{
    [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
    public class PageController : BaseController
    {

        public const string CONTROLLER = "Page";

        private readonly ILogger<PageController> _logger;

        public PageController(IMapper mapper, ILogger<PageController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Display(Name = "مدیریت صفحات")]
        public IActionResult Index()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> GetPage (string models)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetPagesQuery();
            query.Filter = Filter;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<PageVm> GetPageById (Guid pageId)
        {
            var query = new GetPagesQuery();
            query.Id = pageId;
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();

            PageVm pageVm = new PageVm(){
                Id = pageId,
                Action= rslt.Action,
                Controller = rslt.Controller,
                Title = rslt.Title,
                Url = rslt.Url,
            };

            int[] integ = new int[rslt.Roles.Count];

            for (var i=0;i<rslt.Roles.Count;i++)
            {
                integ[i] = (int)rslt.Roles[i];
            }
            ViewBag.RoleDefaultValue = integ;
            //ViewBag.RoleDefaultValue = rslt.Roles.Cast<int>().ToList();
            //ViewBag.RoleDefaultValue =new Array();
            return pageVm;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult GetAllRolesForDropDown()
        {
            return Json(AllRoleTypeEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Edit(Guid pageId)
        {
            var pageData = await GetPageById(pageId);
            return View(pageData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Details(Guid pageId)
        {
            var pageData = await GetPageById(pageId);
            return View(pageData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(PageVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            await Mediator.Send(command, cancellationToken);
            var roles = GetAllRolesForDropDown();
            ViewBag.Roles = roles;
            //return RedirectToAction("Index", "Page");
            return Json(new { redirectToUrl = Url.Action("Index", "Page") });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(PageVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();
            await Mediator.Send(command, cancellationToken);

            return Json(new { redirectToUrl = Url.Action("Index", "Page") });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Delete(Guid pageId)
        {
            var vm = await GetPageById(pageId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(PageDto pageData, CancellationToken cancellationToken)
        {
            ///////changeeeeeeee
            var command = new DeleteDataEntryCommand
            {
                Id = pageData.Id
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}