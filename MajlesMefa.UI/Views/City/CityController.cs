using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.Diagnostics;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Back.UseCases.Commmands.DeleteCityCommand;
using MajlesMefa.Back.UseCases.Queries.GetCitiesQuery;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;

namespace MajlesMefa.UI.Views.City
{
    public class CityController : BaseController
    {

        public const string CONTROLLER = "City";

        private readonly ILogger<CityController> _logger;

        public CityController(IMapper mapper, ILogger<CityController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [Display(Name = "مدیریت استان و حوزه")]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        public IActionResult Index(Guid? id)
        {
            ViewBag.cityId = id;
            if (id == null)
            {
                return View();
            }
            else
            {
                return View("SubIndex");
            }

        }

        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        public IActionResult Create(Guid? cityId)
        {
            CityVm city= new CityVm() { 
                ParentId= cityId,
            };
            return View(city);
        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        public async Task<IActionResult> GetCity(string models, Guid? cityId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetCitiesQuery();
            query.Filter = Filter;
            if (cityId != null)
            {
                query.ParentId = cityId;
            };
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        public async Task<IActionResult> GetCitiesForDropDown(string models, Guid parentId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetCitiesQuery();
            if(Filter.Filter == null || Filter.Filter?.Filters.ToList().Count != 0)
            {
                query.Filter = Filter;
            }
            else
            {
                query.Filter.Filter = null;
            }
            //query.Filter = Filter;
            if (parentId != Guid.Empty)
            {
                query.ParentId = parentId;
            }
            //query.ParentId = parentId;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CityVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToCommand();
            await Mediator.Send(command, cancellationToken);
            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        public async Task<IActionResult> Edit(string name, Guid id, Guid? cityId )
        {
            CityVm city = new CityVm()
            {
                Id = id,
                Name =name,
                ParentId = cityId
            };

            return View(city);
        }

        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(CityVm request, CancellationToken cancellationToken)
        {
            var command = request.ConvertToUpdateCommand();
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        public async Task<CityVm> GetCityById(Guid cityId)
        {
            var query = new GetCityByIdQuery
            {
                Id = cityId,
            };
            var rslt = await Mediator.Send(query);
            CityVm cityVm = new CityVm()
            {
                Id = rslt.Id,
                Name = rslt.Name
            };
            return cityVm;

        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        public async Task<IActionResult> Delete(Guid id, Guid? cityId)
        {
            var vm = await GetCityById(id);
            vm.ParentId = cityId;
            return View(vm);
        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(CityVm cityVm,  CancellationToken cancellationToken)
        {
            var command = new DeleteCityCommand
            {
                Id = cityVm.Id,
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}