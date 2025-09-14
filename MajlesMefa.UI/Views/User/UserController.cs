using AutoMapper;
using Azure.Core;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.UseCases.Commmands.DeleteUseCommand;
using MajlesMefa.Back.UseCases.Queries.GetUserByIdQuery;
using MajlesMefa.Back.UseCases.Queries.GetUsersQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace MajlesMefa.UI.Views.User
{
    [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
    public class UserController : BaseController
    {

        public const string CONTROLLER = "User";

        private readonly ILogger<UserController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public UserController(IMapper mapper, ILogger<UserController> logger,
            ICurrentUserService currentUserService) : base(mapper)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Display(Name = "مدیریت کاربران")]
        public IActionResult Index()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> GetUser(string models)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var cuser = _currentUserService.GetCurrentUser();
            var filteredList = new List<RoleTypeEnum>(){ RoleTypeEnum.Organization};
            if (cuser.Roles.Any(x => x == RoleTypeEnum.Admin))
            {
                filteredList.Add(RoleTypeEnum.MinistryMember);
                filteredList.Add(RoleTypeEnum.MinistryAdmin);
                filteredList.Add(RoleTypeEnum.Admin);
            }
            else if (cuser.Roles.Any(x => x == RoleTypeEnum.MinistryAdmin))
            {
                filteredList.Add(RoleTypeEnum.MinistryMember);
            }
            var query = new GetUsersQuery()
            {
                Roles = filteredList,
            };
            query.Filter = Filter;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<UserVm> GetUserById(Guid userId)
        {
            var query = new GetUserByIdQuery
            {
                UserId = userId,
            };
            var rslt = await Mediator.Send(query);
            UserVm userVm = new UserVm()
            {
                Name= rslt.Name,
                Mobile = rslt.Mobile,
                Username = rslt.Username,
                //Password = rslt.Password,
                CityId = rslt.CityId,
                //CityName = rslt.CityId,
                Role = rslt.Role,
                Email = rslt.Email,
                Id = rslt.Id,
                OrganizationId= rslt.OrganizationId,
                OrganizationName=rslt.OrganizationName,
                OrganizationParentName = rslt.OrganizationParentName,
                OrganizationParentId= rslt.OrganizationParentId,
                CityName = rslt.CityName,
            };
            return userVm;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult Create()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult GetRolesForDropDown()
        {
            return Json(RoleTypeEnumHelper.GetList());
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Edit(Guid userId)
        {
            var userData = await GetUserById(userId);
            if(userData.OrganizationParentId is null)
            {
                userData.OrganizationParentId = userData.OrganizationId;
                userData.OrganizationParentName = userData.OrganizationName;
                userData.OrganizationId = null;
                userData.OrganizationName= null;
            }

            return View(userData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Details(Guid userId)
        {
            var userData = await GetUserById(userId);
            return View(userData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> CreateAsync(UserVm request, CancellationToken cancellationToken)
        {
                var command = request.ConvertToCommand();
                await Mediator.Send(command, cancellationToken);

                return Json(new { redirectToUrl = Url.Action("Index", "User") });
            
            

        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> EditAsync(UserVm request, CancellationToken cancellationToken)
        {
            
                var command = request.ConvertToUpdateCommand();
                await Mediator.Send(command, cancellationToken);

                return Json(new { redirectToUrl = Url.Action("Index", "User") });
            


        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var vm = await GetUserById(userId);

            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(UserDetailDto userData, CancellationToken cancellationToken)
        {
            var command = new DeleteUseCommand
            {
                UserId = userData.Id
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