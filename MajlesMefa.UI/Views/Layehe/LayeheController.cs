using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.UI.Views.Mokatebe;
using MajlesMefa.UI.Views.Shared;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.UI.Models;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Back.ActionFilters;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.UI.Views.Commission;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;

namespace MajlesMefa.UI.Views.Layehe
{
    public class LayeheController : BaseController
    {

        public const string CONTROLLER = "Layehe";

        private readonly ILogger<LayeheController> _logger;

        public LayeheController(IMapper mapper, ILogger<LayeheController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [Display(Name = "مدیریت لوایح")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetLayehe(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Layehe;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 10)]
        public IActionResult GetVazeyatBarresiForDropDown()
        {
            return Json(VazeyatBarresiHelper.GetList());
        }

        public IActionResult GetLayeheOrTarhTypeForDropDown()
        {
            return Json(LayeheTarhTypeEnumHelper.GetList());
        }

        public IActionResult GetNatijeBarresiForDropDown()
        {
            return Json(NatijeBarresiEnumHelper.GetList());
        }

        public IActionResult GetNahveBarresiForDropDown()
        {
            return Json(NahveBarresiEnumHelper.GetList());
        }

        public IActionResult GetNatijeBarresiShoraForDropDown()
        {
            return Json(NatijeBarresiShoraEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<LayeheViewModel> GetLayeheById(Guid layeheId)
        {
            var query = new GetDataEntriesQuery
            {
                DataEntryId = layeheId,
                DataEntryType = DataEntryTypeEnum.Layehe
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();
            LayeheDto layeheDto = (LayeheDto)rslt.MyData;

            //var commissionQuery = new GetDataEntryDetailQuery
            //{
            //    DataEntryId = layeheDto.Commission,
            //    DataEntryType = DataEntryTypeEnum.DastoorJalasatComission
            //};
            //var commissionRslt = await Mediator.Send(commissionQuery);

            LayeheViewModel layehe = new LayeheViewModel()
            {
                CategoryId = rslt.CategoryId,
                CategoryName = rslt.CategoryName,
                CategoryParentName = rslt.CategoryParentName,
                CategoryParentId = rslt.CategoryParentId,
                Moavenats = rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count() > 0 ? true : false,
                Description = rslt.Description,
                Title = rslt.Title,
                NatijeBarresiShoraInt = (int)layeheDto.NatijeBarresiShora,
                NatijeBarresiSahnInt = (int)layeheDto.NatijeBarresiSahn,
                NatijeBarresiCommissionInt = (int)layeheDto.NatijeBarresiCommission,
                TypeInt = (int)layeheDto.Type,
                VazeyatBarresiInt = (int)layeheDto.VazeyatBarresi,
                ElamVosoolPersianDate = layeheDto.ElamVosoolPersianDate,
                EblaghPersianDate = layeheDto.EblaghDate == DateTime.MinValue ? null : layeheDto.EblaghPersianDate,
                BaresiKoliatDarSahnPersianDate = layeheDto.BarresiKoliatDarSahnDate == DateTime.MinValue ? null : layeheDto.BarresiKoliatDarSahnPersianDate,
                Id = layeheId,
                ShomareSabt = layeheDto.ShomareSabt,
                MajorCommissions = layeheDto.MajorCommissions?.Select(x => new Guid(x)).ToList(),
                MinorCommissions = layeheDto.MinorCommissions?.Select(x => new Guid(x)).ToList(),
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                layehe.CategoryParentId = rslt.CategoryId;
                layehe.CategoryParentName = rslt.CategoryName;
                layehe.CategoryId = null;
                layehe.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = layehe.Moavenats;
            ViewBag.MajorCommissionsDefaultValue = layehe.MajorCommissions;
            ViewBag.MinorCommissionsDefaultValue = layehe.MinorCommissions;
            return layehe;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Details(Guid layeheId)
        {
            var layeheData = await GetLayeheById(layeheId);
            return View(layeheData);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin)]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(LayeheViewModel request, CancellationToken cancellationToken)
        {
            var command = LayeheViewModel.ConvertToUpdateCommand(request);
            LayeheDetailDto layehe = JsonConvert.DeserializeObject<LayeheDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            command.DataEntryId = request.Id;
            command.DataEntryData = layehe;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Layehe") });

        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin)]
        public async Task<IActionResult> Edit(Guid layeheId)
        {
            var layeheData = await GetLayeheById(layeheId);
            return View(layeheData);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(LayeheViewModel request, CancellationToken cancellationToken)
        {
            var command = LayeheViewModel.ConvertToCommand(request);
            LayeheDetailDto leyehe = JsonConvert.DeserializeObject<LayeheDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            //LayeheDetailDto leyehe = (LayeheDetailDto)command.DataEntryData;
            command.DataEntryData = leyehe;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Layehe") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin)]
        public async Task<IActionResult> Delete(Guid layeheId)
        {
            var vm = await GetLayeheById(layeheId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(LayeheViewModel request, CancellationToken cancellationToken)
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
