using AutoMapper;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using MajlesMefa.Back.Utilities.Convertor;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;
using IdentityContext.Dtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using Microsoft.AspNetCore.Mvc.Rendering;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.UseCases.Queries.GetUsersDropDownQuery;
using MajlesMefa.Back.UseCases.Commmands.CreateActionReferenceCommand;

namespace MajlesMefa.UI.Views.Mokatebe
{
    public class MokatebeController : BaseController
    {

        public const string CONTROLLER = "Mokatebe";

        private readonly ILogger<MokatebeController> _logger;

        public MokatebeController(IMapper mapper, ILogger<MokatebeController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "مدیریت مکاتبات")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> GetMokatebe (string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetMokatebeDataQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Mokatebe;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth]
        public async Task<MokatebeVm> GetMokatebeById (Guid mokatebeId)
        {
            var query = new GetDataEntriesQuery
            {
                DataEntryId = mokatebeId,
                DataEntryType = DataEntryTypeEnum.Mokatebe
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();
            MokatebeRsltDto mokatebeDto = (MokatebeRsltDto)rslt.MyData;
            MokatebeVm mokatebeVm = new MokatebeVm(){
                Description = rslt.Description,
                Title= rslt.Title,
                CategoryId = rslt.CategoryId,
                CategoryParentId= rslt.CategoryParentId,
                TarikhDabirKhane= mokatebeDto.TarikhDabirKhane == DateTime.MinValue ? null : mokatebeDto.TarikhDabirKhane.ToPersianDate(),
                TarikhNamenamaynde= mokatebeDto.TarikhDabirMarkazi == DateTime.MinValue ? null : mokatebeDto.TarikhDabirMarkazi.ToPersianDate(),
                PasokhDate = mokatebeDto.PasokhDate == DateTime.MinValue ? null : mokatebeDto.PasokhDate.ToPersianDate(),
                SenatorName = rslt.Senator?.Name,
                CategoryName = rslt.CategoryName,
                CategoryParentName= rslt.CategoryParentName,
                SenatorId = rslt.Senator.Id,
                SenatorCity = rslt.Senator?.City,
                HozeEntekhabi = rslt.SenatorHozeEntekhabi,
                Moavenats= rslt.Moavenats?.Select(x => new Guid(x)).ToList(),
                Moshtarak = rslt.Moavenats?.Count() > 0 ? true : false,
                MokatebeData = new MokatebeInputDto()
                {
                    Id = mokatebeId,
                    TarikhDabirKhane = mokatebeDto.TarikhDabirKhane,
                    TarikhNameNamayande= mokatebeDto.TarikhDabirMarkazi,
                    PasokhDate = mokatebeDto.PasokhDate,
                    MokatebeKonandeInt = (int)mokatebeDto.MokatebeKonande,
                    MokatebeType =  mokatebeDto.MokatebeType,
                    Contact =  mokatebeDto.Contact,
                    VaziatPasokh =  mokatebeDto.VaziatPasokh,
                    MokatebeTypeInt= (int)mokatebeDto.MokatebeType,
                    ContactInt= (int)mokatebeDto.Contact,
                    VaziatPasokhInt= (int)mokatebeDto.VaziatPasokh,
                    ShomareDabirkhane = mokatebeDto.ShomareDabirkhane,
                    ShomareDabirkhaneMarkazi = mokatebeDto.ShomareDabirkhaneMarkazi,
                    Amount = mokatebeDto.Amount != null ? (int)mokatebeDto.Amount : 0,
                    PasokhNo = mokatebeDto.PasokhNo
                }
            };
            if (rslt.CategoryParentId is null && rslt.Moavenats is null)
            {
                mokatebeVm.CategoryParentId = rslt.CategoryId;
                mokatebeVm.CategoryParentName = rslt.CategoryName;
                mokatebeVm.CategoryId = null;
                mokatebeVm.CategoryName = null;
            }
            ViewBag.MoavenatsDefaultValue = mokatebeVm.Moavenats;
            return mokatebeVm;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Create()
        {
            MokatebeVm vm = new MokatebeVm {
             UserSelectList = new SelectList(await Mediator.Send(new GetUsersDropDownQuery()), nameof(UserDropDownDto.Id), nameof(UserDropDownDto.Name)),
            };
            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid mokatebeId)
        {
            var mokatebeData = await GetMokatebeById(mokatebeId);
            return View(mokatebeData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Details(Guid mokatebeId)
        {
            var mokatebeData = await GetMokatebeById(mokatebeId);
            
            return View(mokatebeData);
        }

        public IActionResult GetMokatebeTypeForDropDown()
        {
            return Json(MokatebeTypeEnumHelper.GetList());
        }

        public IActionResult GetContactForDropDown()
        {
            return Json(ContactEnumHelper.GetList());
        }

        public IActionResult GetVaziatPasokhForDropDown()
        {
            return Json(ResponseStatusEnumHelper.GetList());
        }

        public IActionResult GetMokatebeKonandeForDropDown()
        {
            return Json(MokatebeKonandeEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(MokatebeVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            MokatebeInputDto mokatebeInput = (MokatebeInputDto)command.DataEntryData;
            if(request.TarikhPeygiri != null)
            {
                mokatebeInput.PeygiriDate = request.TarikhPeygiri.ToMiladiDate();
            }
            mokatebeInput.TarikhDabirKhane = request.TarikhDabirKhane.ToMiladiDate();
            mokatebeInput.TarikhNameNamayande = request.TarikhNamenamaynde.ToMiladiDate();
            mokatebeInput.PasokhDate = request.PasokhDate.ToMiladiDate();
            command.DataEntryData = mokatebeInput;
            Guid dataEntryId = await Mediator.Send(command, cancellationToken);

            if(request.UserId!=null) // erja shode
            {
                var result = await Mediator.Send(new CreateActionReferenceCommand
                {
                    DataEntryId = dataEntryId,
                    Description = "",
                    Action = ActRefTypeEnum.Refer,
                    SetVisibilityForSenator = false,
                    RefrenceUserId = request.UserId,
                    RefType = RefTypeEnum.JahateEstehzar,
                }, cancellationToken);
            }


            return Json(new { redirectToUrl = Url.Action("Index", "Mokatebe") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(MokatebeVm request, CancellationToken cancellationToken)
        {
         var command = request.ConvertToUpdateCommand();
            MokatebeInputDto mokatebeInput = (MokatebeInputDto)command.DataEntryData;
            mokatebeInput.TarikhDabirKhane = request.TarikhDabirKhane.ToMiladiDate();
            mokatebeInput.TarikhNameNamayande = request.TarikhNamenamaynde.ToMiladiDate();
            mokatebeInput.PasokhDate = request.PasokhDate.ToMiladiDate();
            command.DataEntryId = request.MokatebeData.Id;
            command.DataEntryData = mokatebeInput;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Mokatebe") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> Delete(Guid mokatebeId)
        {
            var vm = await GetMokatebeById(mokatebeId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(MokatebeInputDto MokatebeData, CancellationToken cancellationToken)
        {
            var command = new DeleteDataEntryCommand
            {
                Id = MokatebeData.Id
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