using AutoMapper;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.UseCases.Queries.GetLoanBankReportQuery;
using MajlesMefa.Back.UseCases.Queries.GetLoanReportQuery;
using MajlesMefa.Back.UseCases.Queries.GetLoanSenatorReportQuery;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.UI.Views.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Views.Report
{
    public class LoanReportController : BaseController
    {
        public const string CONTROLLER = "LoanReport";
        private readonly ICurrentUserService _currentUserService;

        public LoanReportController(IMapper mapper, ICurrentUserService currentUserService) : base(mapper)
        {
            _currentUserService = currentUserService;
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "گزارش تسهیلات")]
        [Auth]
        public IActionResult Index()
        {
            return View();
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "گزارش تسهیلات")]
        [Auth]
        public IActionResult SenatorIndex()
        {
            return View();
        }


        [RequestLimit(NoOfRequest = 10, Seconds = 5)]
        [Display(Name = "گزارش تسهیلات")]
        [Auth]
        public IActionResult BankIndex()
        {
            return View();
        }


        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> GetLoanReport([FromBody] LoanReportRequestModel requestModel)
        {
            try
            {
                var currentUser = _currentUserService.GetCurrentUser();

                var filter = requestModel?.Filter ?? new TableRequestModel()
                {
                    Skip = 0,
                    Take = 20
                };

                var query = new GetLoanReportQuery
                {
                    DataEntryType = DataEntryTypeEnum.Loan,
                    Filter = filter,
                    CurrentUserId = currentUser.BussinessUserId,
                    ResponseStatus = requestModel?.ResponseStatus ?? ResponseStatusEnum.Inprogress,

                };

                var result = await Mediator.Send(query);
                var rslt = DataSourceResult.GetFromTable(result);

                return Json(rslt);
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { error = "خطا در دریافت داده‌ها" });
            }
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> GetLoanSenatorReport([FromBody] LoanSenatorReportRequestModel requestModel)
        {
            try
            {
                var currentUser = _currentUserService.GetCurrentUser();

                var filter = requestModel?.Filter ?? new TableRequestModel()
                {
                    Skip = 0,
                    Take = 20
                };

                var query = new GetLoanSenatorReportQuery
                {
                    DataEntryType = DataEntryTypeEnum.Loan,
                    Filter = filter,
                    CurrentUserId = currentUser.BussinessUserId,
                    ResponseStatuses = requestModel?.ResponseStatuses ?? new List<ResponseStatusEnum> { ResponseStatusEnum.Inprogress },
                    SenatorName = requestModel?.SenatorName,
                    LoanType = requestModel?.LoanType
                };

                var result = await Mediator.Send(query);
                var rslt = DataSourceResult.GetFromTable(result);

                return Json(rslt);
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { error = "خطا در دریافت داده‌ها" });
            }
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth]
        [HttpPost]
        public async Task<IActionResult> GetLoanBankReport([FromBody] LoanReportRequestModel requestModel)
        {
            try
            {
                var currentUser = _currentUserService.GetCurrentUser();

                var filter = requestModel?.Filter ?? new TableRequestModel()
                {
                    Skip = 0,
                    Take = 20
                };

                var query = new GetLoanBankReportQuery
                {
                    DataEntryType = DataEntryTypeEnum.Loan,
                    Filter = filter,
                    BankUserId = currentUser.BussinessUserId,
                    ResponseStatus = requestModel?.ResponseStatus ?? ResponseStatusEnum.Inprogress,
                };

                var result = await Mediator.Send(query);
                var rslt = DataSourceResult.GetFromTable(result);

                return Json(rslt);
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { error = "خطا در دریافت داده‌ها" });
            }
        }


        [Auth]
        public IActionResult GetResponseStatusForDropDown()
        {
            return Json(ResponseStatusEnumHelper.GetList());
        }

        [Auth]
        public IActionResult GetLoanTypesForDropDown()
        {
            return Json(LoanTypeEnumHelper.GetList());
        }
    }

    public class LoanReportRequestModel
    {
        public TableRequestModel Filter { get; set; }
        public ResponseStatusEnum? ResponseStatus { get; set; }
        public string BankName { get; set; }
        public LoanTypeEnum? LoanType { get; set; }
    }

    public class LoanSenatorReportRequestModel
    {
        public TableRequestModel Filter { get; set; }


        public List<ResponseStatusEnum> ResponseStatuses { get; set; }

        public string SenatorName { get; set; }

        public string BankName { get; set; }
        public LoanTypeEnum? LoanType { get; set; }
    }
}
