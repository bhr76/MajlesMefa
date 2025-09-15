using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.Pagination
{
    public static class PagingExtension
    {
        public static IQueryable<TItem> UsePaging<TItem>(this IQueryable<TItem> query, PagerDto pager) where TItem: IDatedEntity
        {

            if (pager == null)
            {
                return query;
            }
            query = query
                .OrderByDescending(x => x.Created)
                .Skip(pager.Skip)
                .Take(pager.Take);
            return query;
        }
        
        public static IEnumerable<TItem> UsePaging<TItem>(this IEnumerable<TItem> query, PagerDto pager)
        {
            if (pager == null)
            {
                return query;
            }
            query = query
                .Skip(pager.Skip)
                .Take(pager.Take);
            return query;
        }

        public static async Task<ListDto<TItem>> GetListAsync<TItem, TEntity>(this IQueryable<TEntity> query, PagerDto pager, Expression<Func<TEntity, TItem>> f )
            where TEntity : IDatedEntity
        {
            return new ListDto<TItem>()
            {
                Count = await query.CountAsync(),
                Items = await query.UsePaging(pager)
                .Select(f)
                .ToListAsync()
            };
        }
    }
}
