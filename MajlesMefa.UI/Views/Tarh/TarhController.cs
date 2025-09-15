using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.UI.Views.Layehe;
using MajlesMefa.UI.Views.Shared;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.UI.Models;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Back.ActionFilters;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;

namespace MajlesMefa.UI.Views.Tarh
{
    public class TarhController : BaseController
    {

        public const string CONTROLLER = "Tarh";

        private readonly ILogger<TarhController> _logger;

        public TarhController(IMapper mapper, ILogger<TarhController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Display(Name = "مدیریت طرح ها")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetTarh(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Tarh;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult GetNatijeBarresiForDropDown()
        {
            return Json(NatijeBarresiEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult GetVazeyatBarresiForDropDown()
        {
            return Json(VazeyatBarresiHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<TarhViewModel> GetTarhById(Guid tarhId)
        {
            var query = new GetDataEntriesQuery
            {
                DataEntryId = tarhId,
                DataEntryType = DataEntryTypeEnum.Tarh
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();
            TarhDto tarhDto = (TarhDto)rslt.MyData;
            var commissionQuery = new GetDataEntryDetailQuery
            {
                DataEntryId = tarhDto.RelatedComission,
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission
            };
            var commissionRslt = await Mediator.Send(commissionQuery);
            TarhViewModel tarh = new TarhViewModel()
            {
                 Description= rslt.Description,
                SenatorName = rslt.Senator?.Name,
                CategoryId = rslt.CategoryId,
                CategoryName = rslt.CategoryName,
                CategoryParentName = rslt.CategoryParentName,
                CategoryParentId = rslt.CategoryParentId,
                SenatorId = rslt.Senator.Id,
                SenatorCity = rslt.Senator?.City,
                HozeEntekhabi = rslt.SenatorHozeEntekhabi,
                Title = rslt.Title,
                NatijeBarresiShoraNegahbanDescription = tarhDto.NatijeBarresiShoraNegahbanDescription,
                NazarNamayande = tarhDto.NazarNamayande,
                ErsalBeVazir = tarhDto.ErsalBeVazir,
                RelatedComission = tarhDto.RelatedComission,
                RelatedComissionTitle = commissionRslt?.Title,
                Gardeshkar = tarhDto.Gardeshkar,
                GhozarshNahayi = tarhDto.GhozarshNahayi,
                GhozareshMozakerat = tarhDto.GozareshMozakerat,
                HamahangiBaraSherkat = tarhDto.HamahangiBaraSherkat,
                NatijeBarresiShoraNegahbanInt = (int)tarhDto.NatijeBarresiShoraNegahban,
                NatijeBarresiInt = (int)tarhDto.NatijeBarresi,
                VazeyatBarresiInt = (int)tarhDto.VazeyatBarresi,
                MavadTarhInt = (int)tarhDto.MavadTarh,
                KollyatInt = (int)tarhDto.Kollyat,
                Havashi = tarhDto.Havashi,
                Shenase = tarhDto.Shenase,
                Id = tarhId,
                Moavenats = rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count > 0,
                NamayandeghanBaraSherkat = tarhDto.NamayandeghanBaraSherkat,
                NamayandeghanEmzaKonande = tarhDto.NamayandeghanEmzaKonande,
                SavabeghEblagh = tarhDto.SavabeghEblagh
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                tarh.CategoryParentId = rslt.CategoryId;
                tarh.CategoryParentName = rslt.CategoryName;
                tarh.CategoryId = null;
                tarh.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = tarh.Moavenats;
            return tarh;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Details(Guid tarhId)
        {
            var layeheData = await GetTarhById(tarhId);
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
        public async Task<IActionResult> CreateAsync(TarhViewModel request, CancellationToken cancellationToken)
        {
            var command = TarhViewModel.ConvertToCommand(request);
            TarhDetailDto tarh = JsonConvert.DeserializeObject<TarhDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            command.DataEntryData = tarh;
            await Mediator.Send(command, cancellationToken);
            //return RedirectToAction("Index", "Tarh");
            return Json(new { redirectToUrl = Url.Action("Index", "Tarh") });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Edit(Guid tarhId)
        {
            var layeheData = await GetTarhById(tarhId);
            return View(layeheData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(TarhViewModel request, CancellationToken cancellationToken)
        {
            var command = TarhViewModel.ConvertToUpdateCommand(request);
            TarhDetailDto layehe = JsonConvert.DeserializeObject<TarhDetailDto>(JsonConvert.SerializeObject(command.DataEntryData));
            command.DataEntryId = request.Id;
            command.DataEntryData = layehe;
            await Mediator.Send(command, cancellationToken);

            //return RedirectToAction("Index", "Tarh");
            return Json(new { redirectToUrl = Url.Action("Index", "Tarh") });

        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Delete(Guid tarhId)
        {
            var vm = await GetTarhById(tarhId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(TarhViewModel request, CancellationToken cancellationToken)
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
