using AutoMapper;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.UseCases.Commmands.CreateActionReferenceCommand;
using MajlesMefa.Back.UseCases.Commmands.DeleteDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.LoanBatchCommand;
using MajlesMefa.Back.UseCases.Queries.GetBanksQuery;
using MajlesMefa.Back.UseCases.Queries.GetCityDropDown;
using MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetFlatLoanDataEntriesQuery;
using MajlesMefa.Back.UseCases.Queries.GetUsersDropDownQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.Core.ApplicationService.Services.SOAPlus;
using MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MajlesMefa.UI.Views.Loan
{
    public class LoanController : BaseController
    {

        public const string CONTROLLER = "Loan";
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoanController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISoaPlusService _soaPlusService;
        //private readonly ISenatorBudgetRepository _budgetRepository;

        public LoanController(IMapper mapper, ILogger<LoanController> logger, 
            IConfiguration configuration, ICurrentUserService currentUserService,
            ISoaPlusService soaPlusService) : base(mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _currentUserService = currentUserService;
            _soaPlusService = soaPlusService;
            // _budgetRepository = budgetRepository;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "همه ارجاعات")]
        [Auth]
        public IActionResult Index(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "همه ارجاعات فقط خواندنی")]
        [Auth]
        public IActionResult ReadOnlyIndex(Guid? senatorId)
        {
            return View(senatorId);
        }

       

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> GetLoans(string models, Guid? senatorId)
        {
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetFlatLoanDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorId = senatorId;
            try
            {
                var list = await Mediator.Send(query);
                var rslt = DataSourceResult.GetFromTable(list);

                return Json(rslt);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Auth]
        public async Task<LoanVm> GetLoanById(Guid loanId)
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
                    MobileNo = loanDto.MobileNo,
                    NationalNo = loanDto.NationalNo,
                    SuggestedBankName = loanDto.SuggestedBankName,
                    SuggestedBankId = loanDto.SuggestedBankId,
                    LoanType = loanDto.LoanType,
                    LoanTypeInt = (int)loanDto.LoanType,
                    PasokhState = loanDto.PasokhState,
                    PasokhStateInt = (int)loanDto.PasokhState,
                    CityId = loanDto.CityId,
                    ProvinceId = loanDto.ProvinceId,
                    Address = loanDto.Address
                }

            };
            
            return vm;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Create()
        {
     
            LoanVm vm = new LoanVm
            {
             UserSelectList = new SelectList(await Mediator.Send(new GetUsersDropDownQuery()), nameof(UserDropDownDto.Id), nameof(UserDropDownDto.Name)),
            };
            
            var usersQuery = new GetUsersDropDownQuery();
            //var users = await Mediator.Send(usersQuery);
            vm.LoanData = new LoanDtailDto();
            //vm.Users = users.Select(u => new SelectListItem
            //{
            //    Value = u.Id.ToString(),
            //    Text = u.Name,
            //}).ToList();
            return View(vm);
        }


        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> AdminLoan()
        {
            LoanByAdminViewModel vm = new LoanByAdminViewModel
            {
                UserSelectList = new SelectList(await Mediator.Send(new GetUsersDropDownQuery()), nameof(UserDropDownDto.Id), nameof(UserDropDownDto.Name)),
            };

            var usersQuery = new GetUsersDropDownQuery();
            //var users = await Mediator.Send(usersQuery);
            vm.LoanData = new LoanDtailByAdminDto();
            //vm.Users = users.Select(u => new SelectListItem
            //{
            //    Value = u.Id.ToString(),
            //    Text = u.Name,
            //}).ToList();
            return View(vm);
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> Edit(Guid loanId)
        {
            var loanData = await GetLoanById(loanId);
            ViewBag.IsSenator = _currentUserService.GetCurrentUser().Roles.Any(r=>r == RoleTypeEnum.Senator);
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
        public async Task<IActionResult> CreateLoanByAdminAsync(LoanByAdminViewModel request, CancellationToken cancellationToken)
        {
          
            //if (string.IsNullOrEmpty(request.LoanData.Address)
            //    || request.LoanData.ProvinceId == Guid.Empty ||
            //    request.LoanData.CityId == Guid.Empty)
            //{
            //    return BadRequest("آدرس را وارد کنید");

            //}
            if(string.IsNullOrEmpty(request.LoanData.MobileNo))
            {
                request.LoanData.MobileNo = "09351111111";
            }
            request.LoanData.Amount = request.LoanData.Amount.Replace(",", string.Empty);
            var haveMaxGharzolHasaneConfig = long.TryParse(_configuration["maxGharzolHasanePerRequest"], out long gharzolHasaneMaxPerRequest);
            var haveMaxMorabeheConfig = long.TryParse(_configuration["maxMorabehePerRequest"], out long morabeheMaxPerRequest);
            if (!haveMaxGharzolHasaneConfig) { gharzolHasaneMaxPerRequest = 50000000; }
            if (!haveMaxMorabeheConfig) { morabeheMaxPerRequest = 300000000; }
            if (request.LoanData.LoanType == 0)
            {
                return BadRequest("نوع تسهیلات را مشخص کنید");

            }
            else if (request.LoanData.LoanType == LoanTypeEnum.Gharzolhasane && long.Parse(request.LoanData.Amount) > gharzolHasaneMaxPerRequest)
            {
                return BadRequest($"سقف تسهیلات قرض الحسنه برای هر شخص {gharzolHasaneMaxPerRequest} تومان می‌باشد.");
            }
            else if (request.LoanData.LoanType == LoanTypeEnum.Morabehe && long.Parse(request.LoanData.Amount) > morabeheMaxPerRequest)
            {
                return BadRequest($"سقف تسهیلات مرابحه برای هر شخص {morabeheMaxPerRequest} تومان می‌باشد.");
            }

            //var haveshahkarByPassConfig = bool.TryParse(_configuration["shahkarByPass"], out bool shahkarByPass);
            //if (!haveshahkarByPassConfig) { shahkarByPass = true; }
            //if (!shahkarByPass)
            //{
            //    var shahkarInquiry = await _soaPlusService.ShahkarInquiry(new GetShahkarInquiryRequest
            //        (0, request.LoanData.NationalNo, request.LoanData.MobileNo));
            //    if (!shahkarInquiry.Done)
            //    {
            //        return BadRequest(shahkarInquiry.ErrorMessage);
            //    }
            //    if (shahkarInquiry.Result.Response != 200)
            //    {
            //        return BadRequest("شماره ملی و موبایل وارد شده متعلق به یک شخص نیست.");
            //    }
            //}


           // var currentSenator = _currentUserService.GetCurrentUser();


            
            if (request.LoanData.RelatedBankId == Guid.Empty)
            {
                request.LoanData.RelatedBankId = null;
            }
            if (request.LoanData.SuggestedBankId == Guid.Empty)
            {
                request.LoanData.SuggestedBankId = null;
            }
            var command = request.ConvertToCommand();
            LoanDtailByAdminDto loanInput = (LoanDtailByAdminDto)command.DataEntryData;
            command.DataEntryData = loanInput;
            request.LoanData.SenatorBudgetId = null;
            request.LoanData.UserId = null;
            command.IsAdmin = true;
            try
            {
                Guid dataEntryId = await Mediator.Send(command, cancellationToken);
                if (dataEntryId == Guid.Empty)
                {
                    return BadRequest("عملیات به خطا مواجه شده است.");
                }
                var shoraUserIdValue = _configuration["ShoraUserId"];
                if (!Guid.TryParse(shoraUserIdValue, out var shoraUserId) || shoraUserId == Guid.Empty)
                {
                    _logger.LogError("Invalid or missing shoraUserIdValue configuration. Value: {ConfigValue}", shoraUserIdValue);
                    return BadRequest("شناسه کاربر شورا (shoraUserIdValue) در تنظیمات به‌درستی تعریف نشده است.");
                }

                var adminUserIdValue = _configuration["ShoraUserId"];
                if (!Guid.TryParse(adminUserIdValue, out var adminUserId) || adminUserId == Guid.Empty)
                {
                    _logger.LogError("Invalid or missing adminUserIdValue configuration. Value: {ConfigValue}", adminUserIdValue);
                    return BadRequest("شناسه کاربر ادمین (adminUserIdValue) در تنظیمات به‌درستی تعریف نشده است.");
                }

                var referToShora = await Mediator.Send(new CreateActionReferenceCommand
                {
                    DataEntryId = dataEntryId,
                    Description = "",
                    Action = ActRefTypeEnum.Refer,
                    SetVisibilityForSenator = false,
                    RefrenceUserId = shoraUserId,
                    RefType = RefTypeEnum.JahateEstehzar,
                }, cancellationToken);
                // from shora to bank refah
                var referToBankRefah = await Mediator.Send(new CreateActionReferenceCommand
                {
                    DataEntryId = dataEntryId,
                    Description = "",
                    Action = ActRefTypeEnum.Refer,
                    SetVisibilityForSenator = false,
                    //RefrenceUserId = shoraUserId,
                    FromUserId = shoraUserId,
                    RefType = RefTypeEnum.JahateEstehzar,
                }, cancellationToken);


                return Json(new { redirectToUrl = Url.Action("Index", "Loan") });
            }
            catch (Exception ex)
            {

                throw;
            }



        }


        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(LoanVm request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.LoanData.Address) 
                || request.LoanData.ProvinceId == Guid.Empty ||
                request.LoanData.CityId == Guid.Empty)
            {
                return BadRequest("آدرس را وارد کنید");

            }
            request.LoanData.Amount = request.LoanData.Amount.Replace(",", string.Empty);
            var haveMaxGharzolHasaneConfig = long.TryParse(_configuration["maxGharzolHasanePerRequest"], out long gharzolHasaneMaxPerRequest);
            var haveMaxMorabeheConfig = long.TryParse(_configuration["maxMorabehePerRequest"], out long morabeheMaxPerRequest);
            if (!haveMaxGharzolHasaneConfig) { gharzolHasaneMaxPerRequest = 50000000; }
            if (!haveMaxMorabeheConfig) { morabeheMaxPerRequest = 300000000; }
            if (request.LoanData.LoanType == 0)
            {
                return BadRequest("نوع تسهیلات را مشخص کنید");

            }
            else if (request.LoanData.LoanType == LoanTypeEnum.Gharzolhasane && long.Parse(request.LoanData.Amount) > gharzolHasaneMaxPerRequest)
            {
                return BadRequest($"سقف تسهیلات قرض الحسنه برای هر شخص {gharzolHasaneMaxPerRequest} تومان می‌باشد.");
            }
            else if (request.LoanData.LoanType == LoanTypeEnum.Morabehe && long.Parse(request.LoanData.Amount) > morabeheMaxPerRequest)
            {
                return BadRequest($"سقف تسهیلات مرابحه برای هر شخص {morabeheMaxPerRequest} تومان می‌باشد.");
            }

            var haveshahkarByPassConfig = bool.TryParse(_configuration["shahkarByPass"], out bool shahkarByPass);
            if (!haveshahkarByPassConfig) { shahkarByPass = true; }
            if (!shahkarByPass)
            {
                var shahkarInquiry = await _soaPlusService.ShahkarInquiry(new GetShahkarInquiryRequest
                    (0, request.LoanData.NationalNo, request.LoanData.MobileNo));
                if (!shahkarInquiry.Done)
                {
                    return BadRequest(shahkarInquiry.ErrorMessage);
                }
                if (shahkarInquiry.Result.Response != 200)
                {
                    return BadRequest("شماره ملی و موبایل وارد شده متعلق به یک شخص نیست.");
                }
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
            request.LoanData.SenatorBudgetId = null;
            request.LoanData.UserId = null;
            try
            {
                Guid dataEntryId = await Mediator.Send(command, cancellationToken);
                if (dataEntryId == Guid.Empty)
                {
                    return BadRequest("عملیات به خطا مواجه شده است.");
                }
                var shoraUserIdValue = _configuration["ShoraUserId"];
                if (!Guid.TryParse(shoraUserIdValue, out var shoraUserId) || shoraUserId == Guid.Empty)
                {
                    _logger.LogError("Invalid or missing ParlemaniUserId configuration. Value: {ConfigValue}", shoraUserIdValue);
                    return BadRequest("شناسه کاربر پارلمانی (ParlemaniUserId) در تنظیمات به‌درستی تعریف نشده است.");
                }

                var referToShora = await Mediator.Send(new CreateActionReferenceCommand
                {
                    DataEntryId = dataEntryId,
                    Description = "",
                    Action = ActRefTypeEnum.Refer,
                    SetVisibilityForSenator = false,
                    RefrenceUserId = shoraUserId,
                    RefType = RefTypeEnum.JahateEstehzar,
                }, cancellationToken);


                return Json(new { redirectToUrl = Url.Action("Index", "Loan") });
            }
            catch (Exception ex)
            {

                throw;
            }
            

           
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 30)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> EditAsync(LoanVm request, CancellationToken cancellationToken)
        {
            if(request.LoanData.PasokhState == ResponseStatusEnum.Inprogress)
            {
                //edited by Senator
                request.LoanData.Amount = request.LoanData.Amount.Replace(",", string.Empty);
                var haveMaxGharzolHasaneConfig = long.TryParse(_configuration["maxGharzolHasanePerRequest"], out long gharzolHasaneMaxPerRequest);
                var haveMaxMorabeheConfig = long.TryParse(_configuration["maxMorabehePerRequest"], out long morabeheMaxPerRequest);
                if (!haveMaxGharzolHasaneConfig) { gharzolHasaneMaxPerRequest = 50000000; }
                if (!haveMaxMorabeheConfig) { morabeheMaxPerRequest = 300000000; }
                if (request.LoanData.LoanType == 0)
                {
                    return BadRequest("نوع تسهیلات را مشخص کنید");

                }
                else if (request.LoanData.LoanType == LoanTypeEnum.Gharzolhasane && long.Parse(request.LoanData.Amount) > gharzolHasaneMaxPerRequest)
                {
                    return BadRequest("سقف تسهیلات قرض الحسنه برای هر شخص پنجاه میلیون تومان می‌باشد.");
                }
                else if (request.LoanData.LoanType == LoanTypeEnum.Morabehe && long.Parse(request.LoanData.Amount) > morabeheMaxPerRequest)
                {
                    return BadRequest("سقف تسهیلات مرابحه برای هر شخص سیصد میلیون تومان می‌باشد.");
                }
            }

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


        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "ارجاعات جدید")]
        [Auth]
        public IActionResult Unread(Guid? senatorId)
        {
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> GetUnreadLoans(string models, Guid? senatorId)
        {
            var cuser = _currentUserService.GetCurrentUser();
            var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
            var query = new GetFlatLoanDataEntriesQuery();
            query.Filter = Filter;
            query.SenatorId = senatorId;
            query.CurrentUserId = cuser.BussinessUserId;
            var list = await Mediator.Send(query);
            var rslt = DataSourceResult.GetFromTable(list);

            return Json(rslt);
        }

        [HttpPost]
        public async Task<IActionResult> GetProvincesForDropDown()
        {
            try
            {
                var query = new GetCityDropDownQuery(parentId: null);
                var result = await Mediator.Send(query);

                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetCitiesByProvinceForDropDown(Guid provinceId)
        {
            try
            {
                if (provinceId == Guid.Empty)
                {
                    return Json(new { data = new List<GetCityDropDownDto>() });
                }

                var query = new GetCityDropDownQuery(parentId: provinceId);
                var result = await Mediator.Send(query);

                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }


        private bool IsBatchAllowed()
        {
            var username = _currentUserService.GetCurrentUser().UserName;
            return string.Equals(username, "b.refah", StringComparison.OrdinalIgnoreCase);
        }


        [HttpPost]
        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> BatchChangeStatus([FromBody] BatchChangeStatusDto dto, CancellationToken cancellationToken)
        {
            if (!IsBatchAllowed())
                return Forbid(); // یا Unauthorized

            if (dto == null)
                return BadRequest("درخواست نامعتبر است.");

            var command = new BatchChangeStatusCommand(dto);
            var result = await Mediator.Send(command, cancellationToken);

            if (!result)
                return BadRequest("عملیات تغییر وضعیت با شکست مواجه شد یا رکوردی یافت نشد.");

            return Json(new { success = true });
        }

        [HttpPost]
        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        public async Task<IActionResult> BatchRegisterAction([FromBody] BatchRegisterActionDto dto, CancellationToken cancellationToken)
        {
            if (!IsBatchAllowed())
                return Forbid(); // یا Unauthorized
            if (dto == null)
                return BadRequest("درخواست نامعتبر است.");

            var command = new BatchRegisterActionCommand(dto);
            var result = await Mediator.Send(command, cancellationToken);

            if (!result)
                return BadRequest("عملیات ثبت اقدام گروهی با شکست مواجه شد یا رکوردی یافت نشد.");

            return Json(new { success = true });
        }


    }
}