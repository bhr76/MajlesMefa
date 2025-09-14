using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.DeleteMokatebePeygiryCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;
using MajlesMefa.Back.UseCases.Queries.GetOrganizationsQuery;
using MajlesMefa.Back.UseCases.Queries.GetPeigiriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetPeygiriByIdQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.EnumHelper;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.Peygiri
{
    [Auth]
    public class PeygiriController : BaseController
    {

        public const string CONTROLLER = "Peygiri";

        private readonly ILogger<PeygiriController> _logger;

        public PeygiriController(IMapper mapper, ILogger<PeygiriController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Display(Name = "مدیریت پیگیری ها")]
        public async Task<IActionResult> Index(Guid dataEntryId, DataEntryTypeEnum dataEntryType)
        {

            var query = new GetDataEntriesQuery
            {
                DataEntryId = dataEntryId,
              DataEntryType = dataEntryType
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();

            ViewBag.DataEntryTitle = rslt.Title;
            ViewBag.DataEntryId = dataEntryId;
            return View(dataEntryId);
        }

        public async Task<string> getOrganizationById(Guid id)
        {
            var query = new GetOrganizationByIdQuery
            {
                Id = id,
            };
            var organization = await Mediator.Send(query);
            return organization.Name;
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        public async Task<IActionResult> GetPeygiri(string models, Guid dataEntryId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetPeigiriesQuery();
            query.Filter = Filter;
            query.DataEntryId = dataEntryId;
            var list = await Mediator.Send(query);
            //add peygirikonandeName here
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        public IActionResult Create(Guid dataEntryId)
        {
            PeygiriVm peygiri = new PeygiriVm()
            {
                DataEntryId = dataEntryId
            };
            return View(peygiri);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<PeygiriVm> GetPeygiriById(long id)
        {
            var query = new GetPeygiriByIdQuery
            {
               Id=id
            };
            var peygiri = await Mediator.Send(query);

            PeygiriVm peygiriModel = new PeygiriVm()
            {
                Id = id,
                PeygiriDate = peygiri.PeygiriDate,
                PeygiriDatePicker = peygiri.PeygiriDate.ToPersianDate(),
                PeygiriDescription = peygiri.PeygiriDescription,
                PeygiriKonandeId = peygiri.PeygiriKonande,
                PeygiriKonandeTitle = peygiri.PeygiriKonandeTitle,
                PeygiriNumber = peygiri.PeygiriNumber,
            };
            return peygiriModel;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Edit(long id)
        {
            var peygiri = await GetPeygiriById(id);
            return View(peygiri);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(PeygiriVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            var rslt = await Mediator.Send(command, cancellationToken);
            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد. کد پیگیری : " + rslt, MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(PeygiriVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();

            await Mediator.Send(command, cancellationToken);
            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }
        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Delete(long id)
        {
            var peygiri = await GetPeygiriById(id);
            return View(peygiri);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var command = new DeleteMokatebePeygiryCommand
            {
                Id = id
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