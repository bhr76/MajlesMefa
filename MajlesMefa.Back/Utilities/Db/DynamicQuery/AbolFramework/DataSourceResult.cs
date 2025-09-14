using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework
{
    /// <summary>
    /// Describes the result of Kendo DataSource read operation.
    /// </summary>
    [KnownType("GetKnownTypes")]
    public class DataSourceResult
    {
        /// <summary>
        /// Represents a single page of processed data.
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// The total number of records available.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Represents a requested aggregates.
        /// </summary>
        public object Aggregates { get; set; }

        public static DataSourceResult GetFromTable<TEntity>(TableModel<TEntity> table)
        {
            return new DataSourceResult()
            {
                Aggregates = table.Aggregates,
                Data = table.Items,
                Total = table.Count
            };
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Used by the KnownType attribute which is required for WCF serialization support
        /// </summary>
        /// <returns></returns>
        private static Type[] GetKnownTypes()
        {
            var assembly = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .FirstOrDefault(a => a.FullName.StartsWith("DynamicClasses"));

            if (assembly == null)
            {
                return new Type[0];
            }

            return assembly.GetTypes()
                           .Where(t => t.Name.StartsWith("DynamicClass"))
                           .ToArray();
        }
#endif
    }
}
