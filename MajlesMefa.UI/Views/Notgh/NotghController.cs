using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.UI.Views.Mokatebe;
using MajlesMefa.UI.Views.Shared;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.Back.ActionFilters;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;

namespace MajlesMefa.UI.Views.Notgh
{
    public class NotghController : BaseController
    {

        public const string CONTROLLER = "Notgh";

        private readonly ILogger<NotghController> _logger;

        public NotghController(IMapper mapper, ILogger<NotghController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "مدیریت نطق ها")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetNotgh(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Notgh;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<NotghViewModel> GetNotghById(Guid notghId)
        {
            var query = new GetDataEntriesQuery
            {
                DataEntryId = notghId,
                DataEntryType = DataEntryTypeEnum.Notgh
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();
            NotghDto notghDto = (NotghDto)rslt.MyData;
            NotghViewModel notgh = new NotghViewModel()
            {
                Description = rslt.Description,
                PasokhNo = notghDto.PasokhNo,
                PasokhStateInt = (int)notghDto.PasokhState,
                PasokhPersianDate= notghDto.PasokhDate == DateTime.MinValue? null : notghDto.PasokhPersianDate,
                CategoryId = rslt.CategoryId,
                CategoryName = rslt.CategoryName,
                CategoryParentName = rslt.CategoryParentName,
                CategoryParentId = rslt.CategoryParentId,
                SenatorName = rslt.Senator?.Name,
                SenatorId = rslt.Senator.Id,
                SenatorCity = rslt.Senator?.City,
                HozeEntekhabi = rslt.SenatorHozeEntekhabi,
                Title = rslt.Title,
                GardeshErjaat = notghDto.GardeshErjaat,
                Chekide = notghDto.Chekide,
                AnswerFromProUnitPersianDate = notghDto.AnswerFromProUnitPersianDate,
                AnswerFromProUnitNo = notghDto.AnswerFromProUnitNo,
                JalaseAlaniPersianDate = notghDto.JalaseAlaniPersianDate,
                Id = notghId,
                Moavenats = rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count > 0,
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                notgh.CategoryParentId = rslt.CategoryId;
                notgh.CategoryParentName = rslt.CategoryName;
                notgh.CategoryId = null;
                notgh.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = notgh.Moavenats;
            return notgh;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Details(Guid notghId)
        {
            var layeheData = await GetNotghById(notghId);
            return View(layeheData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(NotghViewModel request, CancellationToken cancellationToken)
        {
            var command = NotghViewModel.ConvertToCommand(request);
            NotghDetailDto notgh = JsonConvert.DeserializeObject<NotghDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            command.DataEntryData = notgh;
            await Mediator.Send(command, cancellationToken);
            //return RedirectToAction("Index", "Notgh");
            return Json(new { redirectToUrl = Url.Action("Index", "Notgh") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Edit(Guid notghId)
        {
            var layeheData = await GetNotghById(notghId);
            return View(layeheData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(NotghViewModel request, CancellationToken cancellationToken)
        {
            var command = NotghViewModel.ConvertToUpdateCommand(request);
            NotghDetailDto notgh = JsonConvert.DeserializeObject<NotghDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            command.DataEntryId = request.Id;
            command.DataEntryData = notgh;
            await Mediator.Send(command, cancellationToken);

            // return RedirectToAction("Index", "Notgh");
            return Json(new { redirectToUrl = Url.Action("Index", "Notgh") });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Delete(Guid notghId)
        {
            var vm = await GetNotghById(notghId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(NotghViewModel request, CancellationToken cancellationToken)
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
