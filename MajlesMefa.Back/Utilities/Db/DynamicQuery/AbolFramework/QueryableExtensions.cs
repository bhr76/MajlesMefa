using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies data processing (paging, sorting, filtering and aggregates) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the Kendo DataSource.</param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <param name="aggregates">Specifies the current aggregates.</param>
        /// <returns>A TableResult object populated from the processed IQueryable.</returns>
        public static IQueryable<T> ApplyTable<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<SortModel> sort, FilterModel filter)
        {
            // Filter the data first
            queryable = Filter(queryable, filter);

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            if (take > 0)
            {
                queryable = Page(queryable, take, skip);
            }

            return queryable;
        }

        public static async Task<int> GetTableLengthAsync<T>(this IQueryable<T> queryable, FilterModel filter)
        {
            // Filter the data first
            queryable = Filter(queryable, filter);

            // Calculate the total number of records (needed for paging)
            return await queryable.CountAsync();
        }

        private static TableModel<T> ToTableResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<SortModel> sort, FilterModel filter, IEnumerable<AggregatorModel> aggregates)
        {
            // Filter the data first
            queryable = Filter(queryable, filter);

            // Calculate the total number of records (needed for paging)
            var total = queryable.Count();

            // Calculate the aggregates
            var aggregate = Aggregate(queryable, aggregates);

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            if (take > 0)
            {
                queryable = Page(queryable, take, skip);
            }

            return new TableModel<T>
            {
                Items = queryable.ToList(),
                Count = total,
                Aggregates = aggregate
            };
        }

        private static async Task<TableModel<T>> ToTableResultAsync<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<SortModel> sort, FilterModel filter, IEnumerable<AggregatorModel> aggregates)
        {
            queryable = Filter(queryable, filter);

            // Calculate the total number of records (needed for paging)
            var total = queryable == null ? 0 : await queryable.CountAsync();

            // Calculate the aggregates
            var aggregate = Aggregate(queryable, aggregates);

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            if (take > 0)
            {
                queryable = Page(queryable, take, skip);
            }else if (take == -1)
            {
                queryable = Page(queryable, total, skip);
            }

            var items = await queryable.ToListAsync();

            return new TableModel<T>
            {
                Items = items,
                Count = total,
                Aggregates = aggregate
            };
        }


        /// <summary>
        /// Applies data processing (paging, sorting, filtering and aggregates) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the Kendo DataSource.</param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <param name="aggregates">Specifies the current aggregates.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>


        /// <summary>
        /// Applies data processing (paging, sorting and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the Kendo DataSource.</param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>
        public static TableModel<T> ToTableResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<SortModel> sort, FilterModel filter)
        {
            return queryable.ToTableResult(take, skip, sort, filter, null);
        }

        /// <summary>
        ///  Applies data processing (paging, sorting and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="request">The DataSourceRequest object containing take, skip, order, and filter data.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>
	    public static TableModel<T> ToTableResult<T>(this IQueryable<T> queryable, TableRequestModel request)
        {
            return queryable.ToTableResult(request.Take, request.Skip, request.Sort, request.Filter, null);
        }

        public static TableModel<T> ToTableResult<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.AsQueryable()
                .ToTableResult(enumerable.Count(), 0, new List<SortModel>(), new FilterModel() { Logic = null }, null);
        }

        public static async Task<TableModel<T>> ToTableResultAsync<T>(this IQueryable<T> queryable, TableRequestModel request)
        {
            return await queryable.ToTableResultAsync(request.Take, request.Skip, request.Sort, request.Filter, null);
        }

        public static async Task<TableModel<T>> ToTableResultAsync<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<SortModel> sorts, FilterModel filter)
        {
            return await queryable.ToTableResultAsync(take, skip, sorts, filter, null);
        }

        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, FilterModel filter)
        {
            if (filter?.Logic != null)
            {
                // Collect a flat list of all filters
                var filters = filter.All();

                // Get all filter values as array (needed by the Where method of Dynamic Linq)
                var values = filters.Select(f => f.Value).ToArray();

                // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                var predicate = filter.ToExpression(filters);

                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);
            }

            return queryable;
        }

        private static object Aggregate<T>(IQueryable<T> queryable, IEnumerable<AggregatorModel> aggregates)
        {
            if (aggregates != null && aggregates.Any())
            {
                var objProps = new Dictionary<DynamicProperty, object>();
                var groups = aggregates.GroupBy(g => g.Field);
                Type type;
                foreach (var group in groups)
                {
                    var fieldProps = new Dictionary<DynamicProperty, object>();
                    foreach (var aggregate in group)
                    {
                        var prop = typeof(T).GetProperty(aggregate.Field);
                        var param = Expression.Parameter(typeof(T), "s");
                        var selector = aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                            ? Expression.Lambda(Expression.NotEqual(Expression.MakeMemberAccess(param, prop), Expression.Constant(null, prop.PropertyType)), param)
                            : Expression.Lambda(Expression.MakeMemberAccess(param, prop), param);
                        var mi = aggregate.MethodInfo(typeof(T));
                        if (mi == null)
                            continue;

                        var val = queryable.Provider.Execute(Expression.Call(null, mi,
                            aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                                ? new[] { queryable.Expression }
                                : new[] { queryable.Expression, Expression.Quote(selector) }));

                        fieldProps.Add(new DynamicProperty(aggregate.Aggregate, typeof(object)), val);
                    }
                    type = DynamicClassFactory.CreateType(fieldProps.Keys.ToList());
                    var fieldObj = Activator.CreateInstance(type);
                    foreach (var p in fieldProps.Keys)
                        type.GetProperty(p.Name).SetValue(fieldObj, fieldProps[p], null);
                    objProps.Add(new DynamicProperty(group.Key, fieldObj.GetType()), fieldObj);
                }

                type = DynamicClassFactory.CreateType(objProps.Keys.ToList());

                var obj = Activator.CreateInstance(type);

                foreach (var p in objProps.Keys)
                {
                    type.GetProperty(p.Name).SetValue(obj, objProps[p], null);
                }

                return obj;
            }

            return null;
        }

        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<SortModel> sort)
        {
            if (sort != null && sort.Any())
            {
                // Create ordering expression e.g. Field1 asc, Field2 desc
                var ordering = string.Join(",", sort.Select(s => s.ToExpression()));

                // Use the OrderBy method of Dynamic Linq to sort the data
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        private static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return queryable.Skip(skip).Take(take);
        }
    }
}
