using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.CreateActionReferenceCommand;
using MajlesMefa.Back.UseCases.Commmands.DeleteActionReferenceCommand;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;
using MajlesMefa.Back.UseCases.Queries.GetUsersDropDownQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Views.Khadamat
{
    
    public class KhadamatController : BaseController
    {
        public const string CONTROLLER = "Khadamat";

        private readonly ILogger<KhadamatController> _logger;
        public KhadamatController(IMapper mapper, ILogger<KhadamatController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "مدیریت خدمات")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> GetKhadamat(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Khadamat;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }


        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Create()
        {
            KhadamatVm vm = new KhadamatVm();
            
            return View(vm);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(KhadamatVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            KhadamatDetailDto khadmatInput = (KhadamatDetailDto)command.DataEntryData;
            khadmatInput.TarikhKhedmat = request.TarikhKhedmat.ToMiladiDate();
            command.DataEntryData = khadmatInput;
           await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Khadamat") });
        }


        private async Task<KhadamatVm> GetKhadamatById(Guid khadamatId)
        {
            var query = new GetDataEntryDetailQuery
            {
                DataEntryId = khadamatId,
                DataEntryType = DataEntryTypeEnum.Khadamat
            };
            var res = await Mediator.Send(query);
            

            try
            {
                KhadamatDetailDto dto = (KhadamatDetailDto)res.MyData;
                var vm = new KhadamatVm
                {
                    Description = res.Description,
                    Title = res.Title,
                    TarikhKhedmat = dto.TarikhKhedmat.ToPersianDate(),
                    SenatorName = res.Senator?.Name,
                    SenatorId = res.Senator.Id,
                    SenatorCity = res.Senator?.City,
                    HozeEntekhabi = res.SenatorHozeEntekhabi,

                    KhedmatData = new KhadamatDetailDto
                    {
                        Id = khadamatId,
                        TarikhKhedmat = dto.TarikhKhedmat,
                        Khedmat = dto.Khedmat
                    }

                };

                return vm;
            }
            catch (Exception ex)
            {

                throw;
            }
           
           

        }



        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid khadamatId)
        {
            var khadamatVm = await GetKhadamatById(khadamatId);
            return View(khadamatVm);
        }


        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(KhadamatVm request, CancellationToken cancellationToken)
        {

            var command = request.ConvertToUpdateCommand();
            KhadamatDetailDto khadmatInput = (KhadamatDetailDto)command.DataEntryData;
            khadmatInput.TarikhKhedmat = request.TarikhKhedmat.ToMiladiDate();
            command.DataEntryData = khadmatInput;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Khadamat") });

        }



        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        public async Task<IActionResult> Delete(Guid khadamatId)
        {
            if (khadamatId == Guid.Empty) return BadRequest();
                await Mediator.Send(new DeleteDataEntryCommand
                {
                    Id = khadamatId,
                });

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                refreshGrid = true
            });
        }
    }
}
