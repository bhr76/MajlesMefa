using AutoMapper;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Soval;
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

namespace MajlesMefa.UI.Views.Soal
{
    public class SoalController : BaseController
    {

        public const string CONTROLLER = "Soal";

        private readonly ILogger<SoalController> _logger;

        public SoalController(IMapper mapper, ILogger<SoalController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Display(Name = "مدیریت سوالات")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> GetSoal (string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Soval;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            var x = Json(rslt);
            return x;
        } 

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<SoalVm> GetSoalById (Guid soalId)
        {
            var query = new GetDataEntryDetailQuery
            {
                DataEntryId = soalId,
                DataEntryType = DataEntryTypeEnum.Soval
            };
            var rslt = await Mediator.Send(query);
            SovalDetailDto soalDto = (SovalDetailDto)rslt.MyData;
            var commissionQuery = new GetDataEntryDetailQuery
            {
                DataEntryId = soalDto.Commission,
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission
            };
            var commissionRslt = await Mediator.Send(commissionQuery);
            SoalVm soalVm = new SoalVm(){
                Description = rslt.Description,
                Title= rslt.Title,
                CategoryId = rslt.CategoryId,
                CategoryName = rslt.CategoryName,
                CategoryParentName = rslt.CategoryParentName,
                CategoryParentId = rslt.CategoryParentId,
                SenatorName = rslt.Senator?.Name ?? "",
                SenatorId = rslt.Senator != null ? rslt.Senator.Id : Guid.Empty,
                SenatorCity = rslt.Senator?.City ?? "",
                HozeEntekhabi = rslt.Senator.HozeEntekhabi ?? "",
                KarbargDate = soalDto.KarbargDate.ToPersianDate(),
                Moavenats = rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count > 0,
                SoalData = new SovalDetailDto()
                {
                    Id = soalId,
                    KarbargDate = soalDto.KarbargDate,
                    QuestionStatus = soalDto.QuestionStatus,
                    QuestionStatusInt = (int)soalDto.QuestionStatus,
                    DabirkhaneMakaziNo = soalDto.DabirkhaneMakaziNo,
                    Commission = soalDto.Commission,
                    CommissionTitle = commissionRslt?.Title,
                    BarresiDarCommission = soalDto.BarresiDarCommission,
                    BarresiDarSahn = soalDto.BarresiDarSahn,
                    JalasatDakheli = soalDto.JalasatDakheli,
                }
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                soalVm.CategoryParentId = rslt.CategoryId;
                soalVm.CategoryParentName = rslt.CategoryName;
                soalVm.CategoryId = null;
                soalVm.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = soalVm.Moavenats;
            return soalVm;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid soalId)
        {
            var soalData = await GetSoalById(soalId);
            return View(soalData);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Details(Guid soalId)
        {
            var soalData = await GetSoalById(soalId);
            return View(soalData);
        }

        public IActionResult GetMohlatZamaniForDropDown()
        {
            return Json(MohlatZamaniEnumHelper.GetList());
        }

             public IActionResult GetQuestionStatusForDropDown()
        {
            return Json(QuestionStatusEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(SoalVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            SovalDetailDto soalInput = (SovalDetailDto)command.DataEntryData;
            soalInput.KarbargDate = request.KarbargDate.ToMiladiDate();
            command.DataEntryData = soalInput;
            await Mediator.Send(command, cancellationToken);

            //return RedirectToAction("Index", "Soal");
            return Json(new { redirectToUrl = Url.Action("Index", "Soal") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(SoalVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();
            SovalDetailDto soalInput = (SovalDetailDto)command.DataEntryData;
            soalInput.KarbargDate = request.KarbargDate.ToMiladiDate();
            command.DataEntryId = request.SoalData.Id;
            command.DataEntryData = soalInput;
            await Mediator.Send(command, cancellationToken);
            
            //return RedirectToAction("Index", "Soal");
            return Json(new { redirectToUrl = Url.Action("Index", "Soal") });

            //return Json(new
            //{
            //    Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
            //    RefreshGrid = true
            //});
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Delete(Guid soalId)
        {
            var vm = await GetSoalById(soalId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(SovalDetailDto soalData, CancellationToken cancellationToken)
        {
            var command = new DeleteDataEntryCommand
            {
                Id = soalData.Id
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