using MajlesMefa.Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Abstraction
{
    public interface IDataEntryRepository: IRepository<DataEntryEntity>
    {
        /// <summary>
        /// throws exception if closed
        /// </summary>
        /// <param name="dataEntryId"></param>
        /// <returns></returns>
        Task CheckIsClosedAsync(Guid dataEntryId);
    }
}
