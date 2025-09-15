using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.UI.Views.Shared;
using MajlesMefa.UI.Views.Tarh;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.UI.Models;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Utilities.EnumHelper;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Back.Services.Abstractioin;
using AspNetCoreHero.ToastNotification.Abstractions;
using NToastNotify;
using MajlesMefa.Back.ActionFilters;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;

namespace MajlesMefa.UI.Views.Tazakor
{
    public class TazakorController : BaseController
    {

        public const string CONTROLLER = "Tazakor";
        private readonly ILogger<TazakorController> _logger;

        public TazakorController(IMapper mapper, ILogger<TazakorController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "مدیریت تذکرات")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public IActionResult Index(Guid? senatorId)
        {

            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetTazakor(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Tazakor;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<TazakorViewModel> GetTazakorById(Guid tazakorId)
        {
            var query = new GetDataEntriesQuery
            {
                DataEntryId = tazakorId,
                DataEntryType = DataEntryTypeEnum.Tazakor,
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();
            var tazakor = (TazakorDto)rslt.MyData;
            TazakorViewModel tazakorModel = new TazakorViewModel()
            {
                SenatorName = rslt.Senator?.Name,
                CategoryId = rslt.CategoryId,
                CategoryName = rslt.CategoryName,
                CategoryParentName = rslt.CategoryParentName,
                CategoryParentId = rslt.CategoryParentId,
                SenatorId = rslt.Senator.Id,
                SenatorCity = rslt.Senator?.City,
                HozeEntekhabi = rslt.SenatorHozeEntekhabi,
                Description = rslt.Description,
                Title = rslt.Title,
                TazakorTypeInt = (int)(tazakor.TazakorType),
                GheraatSahnPersianDate = tazakor.GheraatSahnPersianDate,
                PasokhNo = tazakor.PasokhNo,
                PasokhStateInt = (int)tazakor.PasokhState,
                PasokhPersianDate = tazakor.PasokhDate == DateTime.MinValue ? null : tazakor.PasokhPersianDate,
                //PishnevisPersianDate = tazakor.PishnevisPersianDate,
                ShomareName = tazakor.ShomareName,
                Moavenats = rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count > 0,
                VaseleAzInt = (int)tazakor.VaseleAz,
                NameVaselePersianDate = tazakor.NameVaseleDate==DateTime.MinValue?null:tazakor.NameVaselePersianDate,
                NameVaseleNo = tazakor.NameVaseleNo,
                NameVaseleDabirkhaneNo = tazakor.NameVaseleDabirkhaneNo,
                Id = tazakorId,
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                tazakorModel.CategoryParentId = rslt.CategoryId;
                tazakorModel.CategoryParentName = rslt.CategoryName;
                tazakorModel.CategoryId = null;
                tazakorModel.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = tazakorModel.Moavenats;
            return tazakorModel;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Details(Guid tazakorId)
        {
            var layeheData = await GetTazakorById(tazakorId);
            return View(layeheData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(TazakorViewModel request, CancellationToken cancellationToken)
        {
           
            var command = TazakorViewModel.ConvertToCommand(request);
            TazakorDetailDto tazakor = JsonConvert.DeserializeObject<TazakorDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            command.DataEntryData = tazakor;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Tazakor") });
            //return RedirectToAction("Index", "Tazakor");
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Edit(Guid tazakorId)
        {
            var tazakorData = await GetTazakorById(tazakorId);
            return View(tazakorData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(TazakorViewModel request, CancellationToken cancellationToken)
        {
            var command = TazakorViewModel.ConvertToUpdateCommand(request);
            command.DataEntryId = request.Id;
            TazakorDetailDto tazakor = JsonConvert.DeserializeObject<TazakorDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            command.DataEntryData = tazakor;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Tazakor") });

        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Delete(Guid tazakorId)
        {
            var tazakorData = await GetTazakorById(tazakorId);

            return View(tazakorData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(TazakorViewModel request, CancellationToken cancellationToken)
        {
            var command = new DeleteDataEntryCommand
            {
                Id = request.Id
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }
    }
}
