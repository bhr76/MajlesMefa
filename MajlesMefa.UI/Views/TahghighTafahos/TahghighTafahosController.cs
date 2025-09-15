using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.TahghighTafahos;
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

namespace MajlesMefa.UI.Views.TahghighTafahos
{
    public class TahghighTafahosController : BaseController
    {

        public const string CONTROLLER = "TahghighTafahos";

        private readonly ILogger<TahghighTafahosController> _logger;

        public TahghighTafahosController(IMapper mapper, ILogger<TahghighTafahosController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Display(Name = "مدیریت تحقیق و تفحص")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetTahghighTafahos(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.TahghighTafahos;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<TahghighTafahosVm> GetTahghighTafahosById(Guid tahghighTafahosId)
        {
            var query = new GetDataEntryDetailQuery
            {
                DataEntryId = tahghighTafahosId,
                DataEntryType = DataEntryTypeEnum.TahghighTafahos
            };
            var rslt = await Mediator.Send(query);
            TahghighTafahosDetailDto tafahosDto = (TahghighTafahosDetailDto)rslt.MyData;
            var commissionQuery = new GetDataEntryDetailQuery
            {
                DataEntryId = tafahosDto.Commission,
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission
            };
            var commissionRslt = await Mediator.Send(commissionQuery);
            TahghighTafahosVm tahghighTafahosVm = new TahghighTafahosVm()
            {
                Description = rslt.Description,
                Title = rslt.Title,
                CategoryId = rslt.CategoryId,
                CategoryName = rslt.CategoryName,
                CategoryParentName = rslt.CategoryParentName,
                CategoryParentId = rslt.CategoryParentId,
                Date = tafahosDto.Date.ToPersianDate(),
                SenatorName = rslt.Senator?.Name,
                SenatorId = rslt.Senator.Id,
                SenatorCity = rslt.Senator?.City,
                HozeEntekhabi = rslt.Senator.HozeEntekhabi,
                Moavenats = rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count > 0,
                TafahosData = new TahghighTafahosDetailDto()
                {
                    Id = tahghighTafahosId,
                    Date = tafahosDto.Date,
                    ShomareDaryaft = tafahosDto.ShomareDaryaft,
                    ShomareName = tafahosDto.ShomareName,
                    Commission = tafahosDto.Commission,
                    CommissionTitle = commissionRslt?.Title,
                    Mokhatab = tafahosDto.Mokhatab,
                    TahghighTafahosVazyatInt = (int)tafahosDto.TahghighTafahosVazyat,
                    TahghighTafahosSenatorsId = tafahosDto.TahghighTafahosSenatorsId,
                    TahghighTafahosSenatorsName = tafahosDto.TahghighTafahosSenatorsName
                }
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                tahghighTafahosVm.CategoryParentId = rslt.CategoryId;
                tahghighTafahosVm.CategoryParentName = rslt.CategoryName;
                tahghighTafahosVm.CategoryId = null;
                tahghighTafahosVm.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = tahghighTafahosVm.Moavenats;
            ViewBag.SenatorsDefaultValue = tahghighTafahosVm.TafahosData.TahghighTafahosSenatorsId;
            return tahghighTafahosVm;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public IActionResult Create()
        {
            return View();
        }



        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Details(Guid tahghighTafahosId)
        {
            var TafahosData = await GetTahghighTafahosById(tahghighTafahosId);
            return View(TafahosData);
        }

        public IActionResult GetVazyatForDropDown()
        {
            return Json(TahghighTafahosVazyatEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(TahghighTafahosVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            TahghighTafahosDetailDto tafahosInput = (TahghighTafahosDetailDto)command.DataEntryData;
            tafahosInput.Date = request.Date.ToMiladiDate();
            command.DataEntryData = tafahosInput;
            await Mediator.Send(command, cancellationToken);

            // return RedirectToAction("Index", "TahghighTafahos");
            return Json(new { redirectToUrl = Url.Action("Index", "TahghighTafahos") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Edit(Guid tahghighTafahosId)
        {
            var TafahosData = await GetTahghighTafahosById(tahghighTafahosId);
            return View(TafahosData);
        }

        [RequestLimit(NoOfRequest = 20, Seconds = 30)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(TahghighTafahosVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();
            TahghighTafahosDetailDto tafahosInput = (TahghighTafahosDetailDto)command.DataEntryData;
            tafahosInput.Date = request.Date.ToMiladiDate();
            command.DataEntryId = request.TafahosData.Id;
            command.DataEntryData = tafahosInput;
            await Mediator.Send(command, cancellationToken);

            //return RedirectToAction("Index", "TahghighTafahos");
            return Json(new { redirectToUrl = Url.Action("Index", "TahghighTafahos") });

            //return Json(new
            //{
            //    Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
            //    RefreshGrid = true
            //});
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Delete(Guid tahghighTafahosId)
        {
            var vm = await GetTahghighTafahosById(tahghighTafahosId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(MokatebeInputDto TafahosData, CancellationToken cancellationToken)
        {
            var command = new DeleteDataEntryCommand
            {
                Id = TafahosData.Id
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