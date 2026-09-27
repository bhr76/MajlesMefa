using AutoMapper;
using Dapper;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.UI.Views.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MajlesMefa.UI.Views.LoanErrorLog
{
    public class LoanErrorLogController : BaseController
    {
        public const string CONTROLLER = "LoanErrorLog";
        private readonly DapperContext _dapperContext;

        public LoanErrorLogController(IMapper mapper, DapperContext dapperContext) : base(mapper)
        {
            _dapperContext = dapperContext;
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 10)]
        [Display(Name = "خطاهای ثبت تسهیلات")]
        [Auth]
        public IActionResult Index() => View();

        [HttpPost]
        [RequestLimit(NoOfRequest = 60, Seconds = 10)]
        [Auth]
        public async Task<IActionResult> GetLogs(string models, string userName)
        {
            var request = JsonConvert.DeserializeObject<LogGridRequest>(models ?? string.Empty) ?? new LogGridRequest();
            var take = request.Take <= 0 ? 10 : Math.Min(request.Take, 200);
            var skip = Math.Max(request.Skip, 0);
            var name = string.IsNullOrWhiteSpace(userName) ? null : userName.Trim();

            const string where = @"WHERE (@UserName IS NULL OR UserName LIKE N'%' + @UserName + N'%')";
            var parameters = new { UserName = name, Skip = skip, Take = take };
            using var connection = _dapperContext.CreateConnection();
            var total = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(1) FROM dbo.LoanErrorLogs {where}", parameters);
            var items = await connection.QueryAsync<LoanErrorLogRow>($@"SELECT Id, OccurredAtUtc, Action, Category, UserId, UserName, InputJson, ErrorMessage, ExceptionDetails, TraceIdentifier
FROM dbo.LoanErrorLogs
{where}
ORDER BY OccurredAtUtc DESC, Id DESC
OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;", parameters);

            return Json(new { data = items, total });
        }

        private sealed class LogGridRequest
        {
            public int Skip { get; set; }
            public int Take { get; set; }
        }

        private sealed class LoanErrorLogRow
        {
            public long Id { get; set; }
            public DateTime OccurredAtUtc { get; set; }
            public string Action { get; set; }
            public string Category { get; set; }
            public Guid? UserId { get; set; }
            public string UserName { get; set; }
            public string InputJson { get; set; }
            public string ErrorMessage { get; set; }
            public string ExceptionDetails { get; set; }
            public string TraceIdentifier { get; set; }
        }
    }
}
