using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetPeigiriesQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.UI.Views.Peygiri;
using MajlesMefa.UI.Views.Shared;
using MajlesMefa.Back.UseCases.Queries.GetActionRefrencesByDataEntryId;
using MajlesMefa.Back.UseCases.Queries.GetOrganizationDropDown;
using Microsoft.AspNetCore.Mvc.Rendering;
using MajlesMefa.UI.Models;
using MajlesMefa.Back.UseCases.Queries.GetCityDropDown;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.UseCases.Commmands.CreateActionReferenceCommand;
using MajlesMefa.Back.UseCases.Queries.GetUsersDropDownQuery;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.UseCases.Commmands.DeleteActionReferenceCommand;

namespace MajlesMefa.UI.Views.ActionReference
{
    [Auth]
    public class ActionReferenceController : BaseController
    {

        public const string CONTROLLER = "ActionReference";

        private readonly ILogger<ActionReferenceController> _logger;
        private readonly ICurrentUserService _currentUser;

        public ActionReferenceController(IMapper mapper, ILogger<ActionReferenceController> logger,
            ICurrentUserService currentUser) : base(mapper)
        {
            _logger = logger;
            _currentUser= currentUser;
        }

        [RequestLimit(NoOfRequest = 10 , Seconds = 60)]
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

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> ActionIndex(Guid dataEntryId, DataEntryTypeEnum dataEntryType)
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

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> GetActionReference(string models, Guid dataEntryId, ActRefTypeEnum actRefType)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetActionRefrencesByDataEntryIdQuery
            {
                DataEntryId = dataEntryId,
                Filter = Filter,
                ActRefType= actRefType
            };
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpGet]
        public async Task<IActionResult> GetOrganizationByParentId(Guid? itemId)
        {
            if (!itemId.HasValue)
            {
                return NotFound("Error");
            }
            var result = await Mediator.Send(new GetOrganizationDropDownQuery(itemId.Value));
            return Ok(result);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpGet]
        public async Task<IActionResult> GetCityByParentId(Guid? itemId)
        {
            if (!itemId.HasValue)
            {
                return NotFound("Error");
            }
            var result = await Mediator.Send(new GetCityDropDownQuery(itemId.Value));
            return Ok(result);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpGet]
        public async Task<IActionResult> GetUserByFilter(Guid? cityId,Guid? organizationId)
        {
            if (!cityId.HasValue || !organizationId.HasValue)
            {
                return NotFound("Error");
            }
            var result = await Mediator.Send(new GetUsersDropDownQuery
            {
                CityId= cityId,
                OrganizationId= organizationId
            });
            return Ok(result);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> RefrenceTo(Guid dataEntryId)
        {
            var model = new ActionReferenceViewModel
            {
                DataEntryId=dataEntryId,
                UserSelectList= new SelectList(await Mediator.Send(new GetUsersDropDownQuery()), nameof(UserDropDownDto.Id), nameof(UserDropDownDto.Name)),
                // CitySelectList = new SelectList(await Mediator.Send(new GetCityDropDownQuery()), nameof(GetCityDropDownDto.Id), nameof(GetCityDropDownDto.Name)),
                //OrganizationSelectList = new SelectList(await Mediator.Send(new GetOrganizationDropDownQuery()), nameof(GetOrganizationDropDownDto.Id), nameof(GetOrganizationDropDownDto.Name)),
            };
            return PartialView(model);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> ActionTo(Guid dataEntryId)
        {
            var user= _currentUser.GetCurrentUser();
            var model = new ActionViewModel
            {
                DataEntryId = dataEntryId
            };
            return PartialView(model);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        public async Task<IActionResult> ActionTo(ActionViewModel request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new CreateActionReferenceCommand
            {
                DataEntryId = request.DataEntryId,
                Description = request.Description,
                Action = request.ActRefType,
                SetVisibilityForSenator = false,
            }, cancellationToken);
            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }
        [HttpPost]
        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> RefrenceTo(ActionReferenceViewModel request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new CreateActionReferenceCommand
            {
                DataEntryId=request.DataEntryId,
                Description= request.Description,
                Action=ActRefTypeEnum.Refer,
                SetVisibilityForSenator=false,
                RefrenceUserId=request.UserId,
                RefType=request.RefType,
            }, cancellationToken) ;
            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 5, Seconds = 30)]
        public async Task<IActionResult> DeleteRefrence(Guid refrenceId)
        {
            if (refrenceId == Guid.Empty) return BadRequest();
            await Mediator.Send(new DeleteActionReferenceCommand
            {
                Id = refrenceId
            });
            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                refreshGrid = true
            });
        }
    }
}

