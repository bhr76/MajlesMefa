using Microsoft.AspNetCore.Mvc.Filters;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.ActionFilters
{
    public class DataEntryAccessCheckActionFilter : ActionFilterAttribute
    {
        private readonly IBussinessAccessControlService _bussinessAccessControlService;

        public DataEntryAccessCheckActionFilter(IBussinessAccessControlService bussinessAccessControlService)
        {
            _bussinessAccessControlService = bussinessAccessControlService;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor.Parameters
            .Where(c => c.GetType() == typeof(IDataEntryDto))
            .FirstOrDefault() is IDataEntryDto req)
            {
                _bussinessAccessControlService.CheckAccessToDataEntry(req.Id);
            }
            base.OnResultExecuting(context);

        }

    }
}
