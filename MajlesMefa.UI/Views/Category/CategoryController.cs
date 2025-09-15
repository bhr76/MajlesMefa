using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.DeleteCategoryCommand;
using MajlesMefa.Back.UseCases.Queries.GetCategoriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using System.Diagnostics;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Back.ActionFilters;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Limit;

namespace MajlesMefa.UI.Views.Category
{
    public class CategoryController : BaseController
    {

        public const string CONTROLLER = "Category";

        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IMapper mapper, ILogger<CategoryController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [Display(Name = "مدیریت معاونت و موضوع")]
        public IActionResult Index(Guid? id)
        {
            ViewBag.categoryId = id;
            if(id == null)
            {
                return View();
            }
            else
            {
                return View("SubIndex");
            }
           
        }

        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        public IActionResult Create(Guid? categoryId)
        {
            CategoryVm category= new CategoryVm() { 
                ParentId= categoryId,
            };
            return View(category);
        }


        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        public async Task<IActionResult> GetCategory(string models, Guid? categoryId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetCategoriesQuery();
            query.Filter = Filter;
            query.DataEntryType = DataEntryTypeEnum.Mokatebe;
            if (categoryId != null)
            {
                query.ParentId = categoryId;
            };
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        public async Task<IActionResult> GetCategoriesForDropDown(string models, Guid parentId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetCategoriesQuery();
            query.Filter = Filter;
            if(parentId != Guid.Empty)
            {
                query.ParentId = parentId;
            }
            query.DataEntryType = DataEntryTypeEnum.Mokatebe;
            //query.ParentId = parentId;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        public async Task<IActionResult> GetCategoriesForMultiSelect(string models)
        {
            //var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetCategoriesQuery();
            //query.Filter = Filter;
            query.DataEntryType = DataEntryTypeEnum.Mokatebe;
            //query.ParentId = parentId;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryVm request, CancellationToken cancellationToken)
        {
            request.DataEntryType = DataEntryTypeEnum.Mokatebe;
            var command = request.ConvertToCommand();
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        public async Task<IActionResult> Edit(string name, Guid id, Guid? categoryId , bool isCentralOffice)
        {
            CategoryVm category = new CategoryVm()
            {
                Id = id,
                Name =name,
                ParentId = categoryId,
                IsCentralOffice = isCentralOffice,
            };

            return View(category);
        }


        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(CategoryVm request, CancellationToken cancellationToken)
        {
            request.DataEntryType = DataEntryTypeEnum.Mokatebe;
            var command = request.ConvertToUpdateCommand();
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }


        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        public async Task<CategoryVm> GetCategoryById(Guid categoryId)
        {
            var query = new GetCategoryByIdQuery
            {
                Id = categoryId,
                DataEntryType = DataEntryTypeEnum.Mokatebe
            };
            var rslt = await Mediator.Send(query);
            CategoryVm categoryVm = new CategoryVm()
            {
                Id = rslt.Id,
                Name = rslt.Name,
                IsCentralOffice = rslt.IsCentralOffice,
            };
            return categoryVm;

        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin)]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        public async Task<IActionResult> Delete(Guid id, Guid? categoryId)
        {
            var vm = await GetCategoryById(id);
            vm.ParentId = categoryId;
            return View(vm);
        }

        [Auth]
        [RequestLimit(NoOfRequest = 50, Seconds = 60)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(CategoryVm categoryVm,  CancellationToken cancellationToken)
        {
            var command = new DeleteCategoryCommand
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}