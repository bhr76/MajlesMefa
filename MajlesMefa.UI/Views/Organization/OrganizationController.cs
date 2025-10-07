using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.UseCases.Queries.GetCategoriesQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.Diagnostics;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Back.UseCases.Commmands.DeleteOrganizationCommand;
using MajlesMefa.Back.UseCases.Queries.GetOrganizationsQuery;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;

namespace MajlesMefa.UI.Views.Organization
{
    [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
    public class OrganizationController : BaseController
    {

        public const string CONTROLLER = "Organization";

        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(IMapper mapper, ILogger<OrganizationController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Display(Name = "مدیریت سازمان ها")]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public IActionResult Index(Guid? id)
        {
            ViewBag.organizationId = id;
            if (id == null)
            {
                return View();
            }
            else
            {
                return View("SubIndex");
            }
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public IActionResult Create(Guid? organizationId)
        {
            OrganizationVm organization = new OrganizationVm() { 
                ParentId= organizationId,
            };
            return View(organization);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<IActionResult> GetOrganization(string models, Guid? organizationId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetOrganizationsQuery();
            query.Filter = Filter;
            if (organizationId != null)
            {
                query.ParentId = organizationId;
            };
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public async Task<IActionResult> GetOrganizationsForDropDown(string models, Guid parentId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetOrganizationsQuery();
            query.Filter = Filter;
            if(parentId != Guid.Empty)
            {
                query.ParentId = parentId;
            }
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(OrganizationVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Edit(string name, Guid id, Guid? organizationId )
        {
            OrganizationVm category = new OrganizationVm()
            {
                Id = id,
                Name =name,
                ParentId = organizationId
            };

            return View(category);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(OrganizationVm request, CancellationToken cancellationToken)
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
        [Auth(RoleTypeEnum.MinistryMember, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.Admin)]
        public async Task<OrganizationVm> GetOrganizationById(Guid organizationId)
        {
            var query = new GetOrganizationByIdQuery
            {
                Id = organizationId,
            };
            var rslt = await Mediator.Send(query);
            OrganizationVm orgVm = new OrganizationVm()
            {
                Id = rslt.Id,
                Name = rslt.Name
            };
            return orgVm;

        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Delete(Guid id, Guid? organizationId)
        {
            var vm = await GetOrganizationById(id);
            vm.ParentId = organizationId;
            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(CityVm categoryVm,  CancellationToken cancellationToken)
        {
            var command = new DeleteOrganizationCommand
            {
                Id = categoryVm.Id,
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