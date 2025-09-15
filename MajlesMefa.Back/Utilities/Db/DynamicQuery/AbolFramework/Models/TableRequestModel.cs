using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models
{
    public class TableRequestModel
    {
        /// <summary>
        /// Specifies how many items to take.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Specifies the requested sort order.
        /// </summary>
        public IEnumerable<SortModel> Sort { get; set; }

        /// <summary>
        /// Specifies how many items to skip.
        /// </summary>
        public int Skip { get; set; }

        

        /// <summary>
        /// Specifies the requested filter.
        /// </summary>
        public FilterModel? Filter { get; set; }

        public static TableRequestModel Create(int take, int skip, string field, object value, IEnumerable<SortModel> Sorts)
        {
            return new TableRequestModel()
            {
                Take = take,
                Skip = skip,
                Filter = new FilterModel()
                {
                    Field = field,
                    Value = value,
                    Operator = FilterModel.Operators.Equalss
                },

            };
        }

        public static TableRequestModel Create(int take, int skip, List<(string field, object value)> filters, IEnumerable<SortModel> Sorts)
        {
            return new TableRequestModel()
            {
                Take = take,
                Skip = skip,
                Filter = new FilterModel()
                {
                    Filters = filters.Select(f => new FilterModel()
                    {
                        Field = f.field,
                        Operator = FilterModel.Operators.Equalss,
                        Value = f.value
                    })
                }
            };
        }

        public static TableRequestModel Create(string field, object value, int take = 1, int skip = 0)
        {
            return new TableRequestModel()
            {
                Take = take,
                Skip = skip,
                Filter = new FilterModel()
                {
                    Field = field,
                    Operator = FilterModel.Operators.Equalss,
                    Value = value
                }
            };
        }

        public static TableRequestModel Create(string field1, object value1, string field2, object value2, int take = 1, int skip = 0)
        {
            return new TableRequestModel()
            {
                Take = take,
                Skip = skip,
                Filter = new FilterModel()
                {
                    Filters = new FilterModel[]
                    {
                        new FilterModel()
                        {
                            Field = field1,
                            Operator = FilterModel.Operators.Equalss,
                            Value = value1
                        },
                        new FilterModel() {
                            Field = field2,
                            Operator = FilterModel.Operators.Equalss,
                            Value = value2
                        },
                    }
                }
            };
        }

        public static TableRequestModel Create(int take, int skip, List<FilterModel> filters)
        {
            var rslt = new TableRequestModel()
            {
                Take = take,
                Skip = skip,
                Filter = new FilterModel()
                {
                    Filters = filters
                }
            };
            if (filters.Count <= 1)
            {
                rslt.Filter = filters.SingleOrDefault();
            }
            return rslt;
        }
    }
}
