using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.Ezharat
{
    public class EzharatController : BaseController
    {

        public const string CONTROLLER = "Ezharat";

        private readonly ILogger<EzharatController> _logger;

        public EzharatController(IMapper mapper, ILogger<EzharatController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [Display(Name = "مدیریت اظهارات رسانه ای")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> GetEzharat(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.EzhaaratResaneee;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<EzharatVm> GetEzharatById(Guid ezharatId)
        {
            var query = new GetDataEntriesQuery
            {
                DataEntryId = ezharatId,
                DataEntryType = DataEntryTypeEnum.EzhaaratResaneee
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();
            EzhaaratResaneeeDetailDto ezharatDto = (EzhaaratResaneeeDetailDto)rslt.MyData;
            EzharatVm ezharatVm = new EzharatVm()
            {
                Description = rslt.Description,
                Title = rslt.Title,
                CategoryId = rslt.CategoryId,
                CategoryName= rslt.CategoryName,
                CategoryParentName= rslt.CategoryParentName,
                CategoryParentId = rslt.CategoryParentId,
                Date = ezharatDto.Date.ToPersianDate(),
                SenatorName = rslt.Senator?.Name,
                SenatorId = rslt.Senator.Id,
                SenatorCity= rslt.Senator?.City,
                HozeEntekhabi=rslt.SenatorHozeEntekhabi,
                Moavenats = rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count() > 0 ? true : false,
                EzharatData = new EzhaaratResaneeeDetailDto()
                {
                    Id = ezharatId,
                    Manba = ezharatDto.Manba,
                    Date = ezharatDto.Date,
                }
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                ezharatVm.CategoryParentId = rslt.CategoryId;
                ezharatVm.CategoryParentName = rslt.CategoryName;
                ezharatVm.CategoryId = null;
                ezharatVm.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = ezharatVm.Moavenats;
            return ezharatVm;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid ezharatId)
        {
            var ezharatData = await GetEzharatById(ezharatId);
            return View(ezharatData);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Details(Guid ezharatId)
        {
            var ezharatData = await GetEzharatById(ezharatId);
            return View(ezharatData);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(EzharatVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            EzhaaratResaneeeDetailDto ezharatInput = (EzhaaratResaneeeDetailDto)command.DataEntryData;
            ezharatInput.Date = request.Date.ToMiladiDate();
            command.DataEntryData = ezharatInput;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Ezharat") });
            //return RedirectToAction("Index", "Ezharat");
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(EzharatVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();
            EzhaaratResaneeeDetailDto ezharatInput = (EzhaaratResaneeeDetailDto)command.DataEntryData;
            ezharatInput.Date = request.Date.ToMiladiDate();
            command.DataEntryId = request.EzharatData.Id;
            command.DataEntryData = ezharatInput;
            await Mediator.Send(command, cancellationToken);

            return Json(new { redirectToUrl = Url.Action("Index", "Ezharat") });

            //return Json(new
            //{
            //    Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
            //    RefreshGrid = true
            //});
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Delete(Guid ezharatId)
        {
            var vm = await GetEzharatById(ezharatId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(EzhaaratResaneeeDetailDto ezharatData, CancellationToken cancellationToken)
        {
            var command = new DeleteDataEntryCommand
            {
                Id = ezharatData.Id
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}