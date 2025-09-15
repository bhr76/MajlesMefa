using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Molaghat;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.Molaghat
{
    public class MolaghatController : BaseController
    {

        public const string CONTROLLER = "Molaghat";

        private readonly ILogger<MolaghatController> _logger;

        public MolaghatController(IMapper mapper, ILogger<MolaghatController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [Display(Name = "مدیریت ملاقات ها")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [Auth]
        public async Task<IActionResult> GetMolaghat (string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Molaghat;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [Auth]
        public async Task<MolaghatVm> GetMolaghatById (Guid molaghatId)
        {
            var query = new GetDataEntryDetailQuery
            {
                DataEntryId = molaghatId,
                DataEntryType = DataEntryTypeEnum.Molaghat
            };
            var rslt = await Mediator.Send(query);
            MolaghatDtailDto molaghatDto = (MolaghatDtailDto)rslt.MyData;
            var commissionQuery = new GetDataEntryDetailQuery
            {
                DataEntryId = new Guid(rslt.CommissionTitle),
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission
            };
            var commissionRslt = await Mediator.Send(commissionQuery);

            MolaghatVm molaghatVm = new MolaghatVm(){
                Description = rslt.Description,
                //Title= rslt.Title,
                CategoryId = rslt.CategoryId,
                CategoryParentId= rslt.CategoryParentId,
                Date= molaghatDto.Date == DateTime.MinValue? null : molaghatDto.Date.ToPersianDate(),
                PasokhDate = molaghatDto.PasokhDate == DateTime.MinValue ? null : molaghatDto.PasokhDate.ToPersianDate(),
                SenatorName = rslt.Senator?.Name,
                SenatorId = rslt.Senator.Id,
                SenatorCity = rslt.Senator?.City,
                HozeEntekhabi = rslt.Senator.HozeEntekhabi,
                MolaghatData = new MolaghatDtailDto()
                {
                    Id = molaghatId,
                    Commision = molaghatDto.Commision,
                    CommisionTitle = commissionRslt.Title,
                    Date= molaghatDto.Date,
                    Count= molaghatDto.Count,
                    Mahal  = molaghatDto.Mahal,
                    PasokhNo = molaghatDto.PasokhNo,
                    PasokhDate = molaghatDto.PasokhDate,
                    PasokhStateInt = (int)molaghatDto.PasokhState,
                    MolaghatTypeInt = (int)molaghatDto.MolaghatType,
                    IsMolaghatBaVazir = molaghatDto.IsMolaghatBaVazir
                }
            };
            return molaghatVm;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid molaghatId)
        {
            var molaghatData = await GetMolaghatById(molaghatId);
            return View(molaghatData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Details(Guid molaghatId)
        {
            var molaghatData = await GetMolaghatById(molaghatId);
            return View(molaghatData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(MolaghatVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            MolaghatDtailDto molaghatInput = (MolaghatDtailDto)command.DataEntryData;
            molaghatInput.Date = request.Date.ToMiladiDate();
            molaghatInput.PasokhDate = request.PasokhDate.ToMiladiDate();
            command.DataEntryData = molaghatInput;
            await Mediator.Send(command, cancellationToken);

            //return RedirectToAction("Index", "Molaghat");
            return Json(new { redirectToUrl = Url.Action("Index", "Molaghat") });
        }
        

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(MolaghatVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();
            MolaghatDtailDto molaghatInput = (MolaghatDtailDto)command.DataEntryData;
            molaghatInput.Date = request.Date.ToMiladiDate();
            molaghatInput.PasokhDate = request.PasokhDate.ToMiladiDate();
            command.DataEntryId = request.MolaghatData.Id;
            command.DataEntryData = molaghatInput;
            await Mediator.Send(command, cancellationToken);

            return Json(new { redirectToUrl = Url.Action("Index", "Molaghat") });
        }

        //[RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Delete(Guid molaghatId)
        {
            var vm = await GetMolaghatById(molaghatId);

            return View(vm);
        }

        //[RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(MolaghatDtailDto molaghatData, CancellationToken cancellationToken)
        {
            var command = new DeleteDataEntryCommand
            {
                Id = molaghatData.Id
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        public IActionResult GetMolaghatTypesForDropDown()
        {
            return Json(MolaghatTypeEnumHelper.GetList());
        }

        public IActionResult GetMolaghatLocationsForDropDown()
        {
            return Json(MolaghatLocationEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}