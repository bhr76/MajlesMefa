using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class DataEntryRepository : BaseRepository<DataEntryEntity>, IDataEntryRepository
    {
        public DataEntryRepository(RefahMajlesDbContext context) : base(context)
        {
        }

        public async Task CheckIsClosedAsync(Guid dataEntryId)
        {
            var checkIsDataEntryClosed = await _context.ActionReferences
                .Where(x => x.DataEntryId == dataEntryId)
                .AnyAsync(x => x.ActRefType == ActRefTypeEnum.Close);
            if (checkIsDataEntryClosed)
            {
                throw new InvalidOperationException("امکان تغییر دیتا انتری بسته شده وجود ندارد");
            }
        }
    }
}
