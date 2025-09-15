using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Services.Abstractioin
{
    public interface IBussinessAccessControlService
    {
        void CheckAccessToDataEntry(Guid dataEntryId);
        Task CheckAccessToDataEntryAsync(Guid dataEntryId);
    }
}
