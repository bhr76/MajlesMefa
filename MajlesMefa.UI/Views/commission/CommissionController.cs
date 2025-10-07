using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery;
using MajlesMefa.Back.UseCases.Queries.GetSenatorProfileQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.Commission
{
    [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
    public class CommissionController : BaseController
    {

        public const string CONTROLLER = "Commission";

        private readonly ILogger<CommissionController> _logger;

        public CommissionController(IMapper mapper, ILogger<CommissionController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Display(Name = "مدیریت کمیسیون ها")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public IActionResult Index()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth]
        public async Task<IActionResult> GetCommission (string models)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.DataEntryType = DataEntryTypeEnum.DastoorJalasatComission;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetCommissionForMultiSelect(string models)
        {
            var query = new GetDataEntriesQuery();
            query.DataEntryType = DataEntryTypeEnum.DastoorJalasatComission;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<CommissionVm> GetCommissionById (Guid commissionId)
        {
            var query = new GetDataEntryDetailQuery
            {
                DataEntryId = commissionId,
                DataEntryType = DataEntryTypeEnum.DastoorJalasatComission
            };
            var rslt = await Mediator.Send(query);
            DastoorJalasatComissionDetailDto commissionDto = (DastoorJalasatComissionDetailDto)rslt.MyData;
            CommissionVm CommissionVm = new CommissionVm(){
                Description = rslt.Description,
                Title= rslt.Title,
                CategoryId = rslt.CategoryId,
                CategoryParentId= rslt.CategoryParentId,
                Date= commissionDto.DateAndDay == DateTime.MinValue ? null : commissionDto.DateAndDay.ToPersianDate(),
                CommissionData = new DastoorJalasatComissionDetailDto()
                {
                    Id = commissionId,
                    Ghozareshat = commissionDto.Ghozareshat,
                    DateAndDay= commissionDto.DateAndDay,
                    HasRelation = commissionDto.HasRelation,
                }
            };
            return CommissionVm;
        }


        public async Task<IActionResult> GetMembers(Guid commissionId)
        {
            var query = new GetSenatorProfileQuery();
            query.CommissionId = commissionId;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }
        

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Edit(Guid commissionId)
        {
            var commissionData = await GetCommissionById(commissionId);
            return View(commissionData);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Details(Guid commissionId)
        {
            var commissionData = await GetCommissionById(commissionId);
            return View(commissionData);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CommissionVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            DastoorJalasatComissionDetailDto commissionInput = (DastoorJalasatComissionDetailDto)command.DataEntryData;
            commissionInput.DateAndDay = request.Date.ToMiladiDate();
            command.DataEntryData = commissionInput;
            await Mediator.Send(command, cancellationToken);

            //return RedirectToAction("Index", "Commission");
            return Json(new { redirectToUrl = Url.Action("Index", "Commission") });
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(CommissionVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();
            DastoorJalasatComissionDetailDto commissionInput = (DastoorJalasatComissionDetailDto)command.DataEntryData;
            commissionInput.DateAndDay = request.Date.ToMiladiDate();
            command.DataEntryId = request.CommissionData.Id;
            command.DataEntryData = commissionInput;
            await Mediator.Send(command, cancellationToken);
            
           // return RedirectToAction("Index", "Commission");
            return Json(new { redirectToUrl = Url.Action("Index", "Commission") });
            //return Json(new
            //{
            //    Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
            //    RefreshGrid = true
            //});
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> Delete(Guid commissionId)
        {
            var vm = await GetCommissionById(commissionId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 60)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(DastoorJalasatComissionDetailDto commissionData, CancellationToken cancellationToken)
        {
            if (commissionData.HasRelation)
            {
                return Json(new
                {
                    Message = Message.Show("کمیسیون انتخاب شده دارای وابستگی میباشد و قابل حذف نیست.", MessageType.Error),
                    RefreshGrid = true
                });
            }
            else
            {
                var command = new DeleteDataEntryCommand
                {
                    Id = commissionData.Id
                };
                await Mediator.Send(command, cancellationToken);

                return Json(new
                {
                    Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                    RefreshGrid = true
                });
            }
            
        }


        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}