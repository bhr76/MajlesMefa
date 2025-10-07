using AutoMapper;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.UseCases.Commmands.CreateSenatorBudgetCommand;
using MajlesMefa.Back.UseCases.Commmands.DeleteSenatorBudgetCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateSenatorBudgetCommand;
using MajlesMefa.Back.UseCases.Queries.GetSenatorBudgetQuery;
using MajlesMefa.Back.UseCases.Queries.GetSenatorProfileQuery;
using MajlesMefa.Back.UseCases.Queries.GetUsersDropDownQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.SenatorBudget
{
    public class SenatorBudgetController : BaseController
    {
        public const string CONTROLLER = "SenatorBudget";

        private readonly ILogger<SenatorBudgetController> _logger;

        public SenatorBudgetController(IMapper mapper, ILogger<SenatorBudgetController> logger) : base(mapper)
        {
            _logger = logger;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Display(Name = "مدیریت تعیین بودجه")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> GetSenatorBudgets(string models, Guid? senatorId)
        {
            var filter = JsonConvert.DeserializeObject<TableRequestModel>(models) ?? new TableRequestModel();
            var query = new GetSenatorBudgetQuery();
            query.Filter = filter;

            // Apply senatorId filter if provided
            if (senatorId.HasValue && senatorId.Value != Guid.Empty)
            {
                if (filter.Filter == null)
                {
                    filter.Filter = new FilterModel();
                }

                var senatorFilter = new FilterModel
                {
                    Field = nameof(SenatorBudgetDto.SenatorId),
                    Operator = FilterModel.Operators.Equalss,
                    Value = senatorId.Value
                };

                if (filter.Filter.Filters != null && filter.Filter.Filters.Any())
                {
                    var filters = filter.Filter.Filters.ToList();
                    filters.Add(senatorFilter);
                    filter.Filter.Filters = filters;
                }
                else
                {
                    filter.Filter = senatorFilter;
                }

                query.Filter = filter;
            }

            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Create(Guid? senatorId)
        {
            var vm = new SenatorBudgetVm();
            if (senatorId.HasValue)
            {
                vm.SenatorId = senatorId.Value;
            }
            await FillDropDownsAsync(vm);
            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(SenatorBudgetVm request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await FillDropDownsAsync(request);
                return View("Create", request);
            }

            var command = new CreateSenatorBudgetCommand()
            {
                Amount = request.Amount,
                SenatorId = request.SenatorId,
                UserId = request.UserId,
                ExecDate = request.ExecDate,
                LoanType = request.RequestLoanType
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new { redirectToUrl = Url.Action(nameof(Index), CONTROLLER) });
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid id)
        {
            var vm = await GetSenatorBudgetById(id);
            if (vm == null) return NotFound();

            await FillDropDownsAsync(vm);
            return View(vm);
          }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(SenatorBudgetVm request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await FillDropDownsAsync(request);
                return View("Edit", request);
            }

            var command = new UpdateSenatorBudgetCommand()
            {
                Id = request.Id!.Value,
                Amount = request.Amount,
                SenatorId = request.SenatorId,
                UserId = request.UserId,
                ExecDate = request.ExecDate,
                LoanType = request.RequestLoanType
            };

            var result = await Mediator.Send(command, cancellationToken);
            if (!result.Status)
            {
                return BadRequest(new { title = result.Reason });
            }

            return Json(new { redirectToUrl = Url.Action(nameof(Index), CONTROLLER) });
        }
                
        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<SenatorBudgetVm> GetSenatorBudgetById(Guid senatorBudgetId)
        {
            var filter = TableRequestModel.Create(nameof(SenatorBudgetDto.Id), senatorBudgetId, take: 1, skip: 0);
            var query = new GetSenatorBudgetQuery { Filter = filter };

            var result = await Mediator.Send(query);
            var dto = result.Items.SingleOrDefault();

            if (dto == null) return null;

            var vm = new SenatorBudgetVm
            {
                Id = dto.Id,
                SenatorId = dto.SenatorId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                ExecDate = dto.ExecDate,
                RequestLoanType = dto.SenatorRequestLoanType    
            };

            return vm;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vm = await GetSenatorBudgetById(id);
            if (vm == null) return NotFound();

            await FillDropDownsAsync(vm);
            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(SenatorBudgetVm request, CancellationToken cancellationToken)
        {
            var command = new DeleteSenatorBudgetCommand
            {
                Id = request.Id.Value
            };
            await Mediator.Send(command, cancellationToken);

            return Json(new
            {
                Message = Message.Show("عملیات با موفقیت انجام شد.", MessageType.Success),
                RefreshGrid = true
            });
        }

        //[RequestLimit(NoOfRequest = 15, Seconds = 10)]
        //[Display(Name = "جزئیات تعیین بودجه")]
        //[Auth]
        //public async Task<IActionResult> Details(Guid id)
        //{
        //    var vm = await GetSenatorBudgetById(id);
        //    if (vm == null) return NotFound();

        //    await FillDropDownsAsync(vm);
        //    return View(vm);
        //}

        private async Task FillDropDownsAsync(SenatorBudgetVm model)
        {
            // Fill Senators dropdown
            var senatorsQuery = new GetUsersDropDownQuery { Role = RoleTypeEnum.Senator };
            var senators = await Mediator.Send(senatorsQuery);
            model.Senators = senators.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = s.Id == model.SenatorId
            }).ToList();

            // Fill Users dropdown (all users)
            var usersQuery = new GetUsersDropDownQuery();
            var users = await Mediator.Send(usersQuery);
            model.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name,
                Selected = u.Id == model.UserId
            }).ToList();
            
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        public IActionResult GetAllRequestLoanTypesForDropDown()
        {
            return Json(SenatorRequestLoanTypeEnumHelper.GetList());
        }

        [Auth]
        public async Task<IActionResult> GetSenatorsForDropDown(string models)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetSenatorProfileQuery();
            if (Filter.Filter == null || Filter.Filter?.Filters.ToList().Count != 0)
            {
                query.Filter = Filter;
            }
            else
            {
                query.Filter.Filter = null;
            }
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);
            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}