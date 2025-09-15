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
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.UseCases.Queries.GetSenatorProfileQuery;
using MajlesMefa.Back.UseCases.Queries.GetBanksQuery;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Dtos;

namespace MajlesMefa.UI.Views.Loan
{
    public class LoanController : BaseController
    {

        public const string CONTROLLER = "Loan";
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoanController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoanController(IMapper mapper, ILogger<LoanController> logger, IConfiguration configuration, ICurrentUserService currentUserService) : base(mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "مدیریت تسهیلات")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> GetLoans (string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorIdId = senatorId;
            query.DataEntryType = DataEntryTypeEnum.Loan;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth]
        public async Task<LoanVm> GetLoanById (Guid loanId)
        {
            var query = new GetDataEntriesQuery
            {
                DataEntryId = loanId,
                DataEntryType = DataEntryTypeEnum.Loan
            };
            var list = await Mediator.Send(query);
            var rslt = list.Items.SingleOrDefault();
            LoanDtailDto loanDto = (LoanDtailDto)rslt.MyData;
            var vm = new LoanVm
            {
                Description = rslt.Description,
                SenatorName = rslt.Senator?.Name,
                SenatorId = rslt.Senator.Id,
                SenatorCity = rslt.Senator?.City,
                HozeEntekhabi = rslt.SenatorHozeEntekhabi,

                LoanData = new LoanDtailDto
                {
                    Id = loanId,
                    Amount = loanDto.Amount,
                    FullName = loanDto.FullName,
                    MobileNo= loanDto.MobileNo,
                    NationalNo = loanDto.NationalNo,
                    SuggestedBankName= loanDto.SuggestedBankName,
                    SuggestedBankId= loanDto.SuggestedBankId,
                    LoanType= loanDto.LoanType,
                    LoanTypeInt= (int)loanDto.LoanType,
                    PasokhState = loanDto.PasokhState,
                    PasokhStateInt = (int)loanDto.PasokhState
                }

            };
            
            return vm;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Create()
        {
            LoanVm vm = new LoanVm {
             UserSelectList = new SelectList(await Mediator.Send(new GetUsersDropDownQuery()), nameof(UserDropDownDto.Id), nameof(UserDropDownDto.Name)),
            };
            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid loanId)
        {
            var loanData = await GetLoanById(loanId);
            return View(loanData);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Details(Guid loanId)
        {
            var mokatebeData = await GetLoanById(loanId);
            
            return View(mokatebeData);
        }

        [Auth]
        public async Task<IActionResult> GetBanksForDropDown(string models)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetBanksQuery();
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
        [Auth]
        public IActionResult GetLoanTypesForDropDown()
        {
            return Json(LoanTypeEnumHelper.GetList());
        }

        [Auth]
        public IActionResult GetPasokhStatesForDropDown()
        {
            return Json(ResponseStatusEnumHelper.GetList());
        }

        public IActionResult GetVaziatPasokhForDropDown()
        {
            return Json(ResponseStatusEnumHelper.GetList());
        }


        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(LoanVm request, CancellationToken cancellationToken)
        {
            request.LoanData.Amount = request.LoanData.Amount.Replace(",", string.Empty);
            if (request.LoanData.LoanType == 0)
            {
                return BadRequest("نوع تسهیلات را مشخص کنید");
               
            }
            else if(request.LoanData.LoanType == LoanTypeEnum.Gharzolhasane && long.Parse(request.LoanData.Amount) > 50000000)
            {
                return BadRequest("سقف تسهیلات قرض الحسنه برای هر شخص پنجاه میلیون تومان می‌باشد.");
            }
            else if (request.LoanData.LoanType == LoanTypeEnum.Morabehe && long.Parse(request.LoanData.Amount) > 300000000)
            {
                return BadRequest("سقف تسهیلات مرابحه برای هر شخص سیصد میلیون تومان می‌باشد.");
            }
            var currentSenator = _currentUserService.GetCurrentUser();
            request.SenatorId = currentSenator.BussinessUserId;
            if (request.LoanData.RelatedBankId == Guid.Empty)
            {
                request.LoanData.RelatedBankId = null;
            }
            if (request.LoanData.SuggestedBankId == Guid.Empty)
            {
                request.LoanData.SuggestedBankId = null;
            }
            var command = request.ConvertToCommand();
            LoanDtailDto loanInput = (LoanDtailDto)command.DataEntryData;
            
            command.DataEntryData = loanInput;
            Guid dataEntryId = await Mediator.Send(command, cancellationToken);
            if(dataEntryId == Guid.Empty)
            {
                return BadRequest("عملیات به خطا مواجه شده است.");
            }
            var referToShora = await Mediator.Send(new CreateActionReferenceCommand
            {
                DataEntryId = dataEntryId,
                Description = "",
                Action = ActRefTypeEnum.Refer,
                SetVisibilityForSenator = false,
                RefrenceUserId = new Guid(_configuration.GetSection("ParlemaniUserId").Value),
                RefType = RefTypeEnum.JahateEstehzar,
            }, cancellationToken);


            return Json(new { redirectToUrl = Url.Action("Index", "Loan") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(LoanVm request, CancellationToken cancellationToken)
        {
            request.LoanData.Amount = request.LoanData.Amount.Replace(",", string.Empty);
            var command = request.ConvertToUpdateCommand();
            LoanDtailDto loanDetails = (LoanDtailDto)command.DataEntryData;
            command.DataEntryId = request.LoanData.Id;
            command.DataEntryData = loanDetails;
            await Mediator.Send(command, cancellationToken);
            return Json(new { redirectToUrl = Url.Action("Index", "Loan") });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> Delete(Guid loanId)
        {
            var vm = await GetLoanById(loanId);

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